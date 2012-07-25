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
using System.Reflection;
using Remotion.TypePipe.MutableReflection;

namespace Remotion.TypePipe.UnitTests.MutableReflection
{
  public class CustomTypeStub : CustomType
  {
    public CustomTypeStub (
        IMemberSelector memberSelector,
        Type underlyingSystemType,
        Type baseType,
        TypeAttributes typeAttributes,
        string name,
        string @namespace,
        string fullName)
        : base (memberSelector, underlyingSystemType, baseType, typeAttributes, name, @namespace, fullName)
    {
    }

    public IEnumerable<Type> Interfaces;
    public IEnumerable<FieldInfo> Fields;
    public IEnumerable<ConstructorInfo> Constructors;
    public IEnumerable<MethodInfo> Methods;

    protected override IEnumerable<Type> GetAllInterfaces ()
    {
      return Interfaces;
    }

    protected override IEnumerable<FieldInfo> GetAllFields ()
    {
      return Fields;
    }

    protected override IEnumerable<ConstructorInfo> GetAllConstructors ()
    {
      return Constructors;
    }

    public override bool IsEquivalentTo (Type type)
    {
      throw new NotImplementedException();
    }

    //protected override IEnumerable<MethodInfo> GetAllMethods ()
    //{
    //  return AllMethods;
    //}

    // TODO Delete this blub
    protected override MethodInfo GetMethodImpl (string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
    {
      throw new NotImplementedException();
    }

    public override MethodInfo[] GetMethods (BindingFlags bindingAttr)
    {
      throw new NotImplementedException();
    }
  }
}