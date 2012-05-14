﻿// Copyright (c) rubicon IT GmbH, www.rubicon.eu
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
using System.Reflection;
using Remotion.Utilities;

namespace Remotion.TypePipe.MutableReflection
{
  /// <summary>
  /// Provides utility functions for working with method names.
  /// </summary>
  public static class MethodNameUtility
  {
    public static string GetExplicitOverrideMethodName (MethodInfo overriddenMethod)
    {
      ArgumentUtility.CheckNotNull ("overriddenMethod", overriddenMethod);

      return overriddenMethod.DeclaringType.FullName + "_" + overriddenMethod.Name;
    }
  }
}