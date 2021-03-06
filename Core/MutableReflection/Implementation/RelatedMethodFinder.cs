// Copyright (c) rubicon IT GmbH, www.rubicon.eu
//
// See the NOTICE file distributed with this work for additional information
// regarding copyright ownership.  rubicon licenses this file to you under 
// the Apache License, Version 2.0 (the "License"); you may not use this 
// file except in compliance with the License.  You may obtain a copy of the 
// License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the 
// License for the specific language governing permissions and limitations
// under the License.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Remotion.FunctionalProgramming;
using Remotion.TypePipe.MutableReflection.MemberSignatures;
using Remotion.Utilities;

namespace Remotion.TypePipe.MutableReflection.Implementation
{
  /// <summary>
  /// Provides useful methods for investigating method overrides.
  /// </summary>
  /// <threadsafety static="true" instance="true"/>
  public class RelatedMethodFinder : IRelatedMethodFinder
  {
    private class TypePipeMethodInfoEqualityComparer : IEqualityComparer<MethodInfo>
    {
      private readonly IEqualityComparer<MethodInfo> _innerEqualityComparer = MemberInfoEqualityComparer<MethodInfo>.Instance;

      public bool Equals (MethodInfo x, MethodInfo y)
      {
        if (x is CustomMethodInfo && y is CustomMethodInfo)
          return x.Equals (y);
        if (!(x is CustomMethodInfo) && !(y is CustomMethodInfo))
          return _innerEqualityComparer.Equals (x, y);
        return false;
      }

      public int GetHashCode (MethodInfo obj)
      {
        return _innerEqualityComparer.GetHashCode (obj);
      }
    }

    private static readonly IEqualityComparer<MethodInfo> s_memberInfoEqualityComparer = new TypePipeMethodInfoEqualityComparer();

    /// <inheritdoc />
    public MethodInfo GetMostDerivedVirtualMethod (string name, MethodSignature signature, Type typeToStartSearch)
    {
      ArgumentUtility.CheckNotNullOrEmpty ("name", name);
      ArgumentUtility.CheckNotNull ("signature", signature);
      ArgumentUtility.CheckNotNull ("typeToStartSearch", typeToStartSearch);

      Func<MethodInfo, bool> predicate = m => m.IsVirtual && m.Name == name && MethodSignature.Create (m).Equals (signature);
      return FirstOrDefaultFromOrderedBaseMethods (typeToStartSearch, predicate);
    }

    /// <inheritdoc />
    public MethodInfo GetMostDerivedOverride (MethodInfo baseDefinition, Type typeToStartSearch)
    {
      ArgumentUtility.CheckNotNull ("baseDefinition", baseDefinition);
      ArgumentUtility.CheckNotNull ("typeToStartSearch", typeToStartSearch);
      Assertion.DebugAssert (s_memberInfoEqualityComparer.Equals (baseDefinition, MethodBaseDefinitionCache.GetBaseDefinition (baseDefinition)));
      
      Func<MethodInfo, bool> predicate = m => 
          s_memberInfoEqualityComparer.Equals (baseDefinition, MethodBaseDefinitionCache.GetBaseDefinition (m));
      return FirstOrDefaultFromOrderedBaseMethods (typeToStartSearch, predicate);
    }

    /// <inheritdoc />
    public MethodInfo GetBaseMethod (MethodInfo method)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      Assertion.IsNotNull (method.DeclaringType);

      var baseDefinition = MethodBaseDefinitionCache.GetBaseDefinition (method);
      if (baseDefinition.DeclaringType.BaseType == null)
        return null;

      return GetMostDerivedOverride (baseDefinition, method.DeclaringType.BaseType);
    }

    /// <inheritdoc />
    public bool IsShadowed(MethodInfo baseDefinition, IEnumerable<MethodInfo> shadowingCandidates)
    {
      ArgumentUtility.CheckNotNull ("baseDefinition", baseDefinition);
      ArgumentUtility.CheckNotNull ("shadowingCandidates", shadowingCandidates);
      Assertion.DebugAssert (s_memberInfoEqualityComparer.Equals (baseDefinition, MethodBaseDefinitionCache.GetBaseDefinition (baseDefinition)));

      return shadowingCandidates.Any (
          m => m.Name == baseDefinition.Name
               && MethodSignature.AreEqual (m, baseDefinition)
               && baseDefinition.DeclaringType.IsTypePipeAssignableFrom (m.DeclaringType.BaseType)
               && !s_memberInfoEqualityComparer.Equals (baseDefinition, MethodBaseDefinitionCache.GetBaseDefinition (m)));
    }

    /// <inheritdoc />
    public MutableMethodInfo GetOverride (MethodInfo baseDefinition, IEnumerable<MutableMethodInfo> overrideCandidates)
    {
      ArgumentUtility.CheckNotNull ("baseDefinition", baseDefinition);
      ArgumentUtility.CheckNotNull ("overrideCandidates", overrideCandidates);
      Assertion.DebugAssert (s_memberInfoEqualityComparer.Equals (baseDefinition, MethodBaseDefinitionCache.GetBaseDefinition (baseDefinition)));

      return overrideCandidates.SingleOrDefault (
          m => s_memberInfoEqualityComparer.Equals (baseDefinition, MethodBaseDefinitionCache.GetBaseDefinition (m))
               || m.AddedExplicitBaseDefinitions.Contains (baseDefinition, s_memberInfoEqualityComparer));
    }

    private MethodInfo FirstOrDefaultFromOrderedBaseMethods (Type typeToStartSearch, Func<MethodInfo, bool> predicate)
    {
      var baseTypeSequence = EnumerableExtensions.CreateSequence (typeToStartSearch, t => t.BaseType);
      var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

      return baseTypeSequence.SelectMany (type => type.GetMethods (bindingFlags)).FirstOrDefault (predicate);
    }
  }
}