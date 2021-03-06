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
using NUnit.Framework;
using Remotion.Development.UnitTesting.Reflection;
using Remotion.TypePipe.Expressions.ReflectionAdapters;

namespace Remotion.TypePipe.UnitTests.Expressions.ReflectionAdapters
{
  [TestFixture]
  public class NonVirtualCallMethodInfoAdapterTest
  {
    [Test]
    public void Adapt_Method ()
    {
      var method = ReflectionObjectMother.GetSomeMethod();

      var adapter = NonVirtualCallMethodInfoAdapter.Adapt (method);

      Assert.That (adapter.AdaptedMethod, Is.SameAs (method));
    }

    [Test]
    public void Adapt_Constructor ()
    {
      var ctor = ReflectionObjectMother.GetSomeConstructor();

      var adapter = NonVirtualCallMethodInfoAdapter.Adapt (ctor);

      Assert.That (adapter.AdaptedMethod, Is.TypeOf<ConstructorAsMethodInfoAdapter>());
      var constructorAsMethodInfoAdapter = (ConstructorAsMethodInfoAdapter) adapter.AdaptedMethod;
      Assert.That (constructorAsMethodInfoAdapter.AdaptedConstructor, Is.SameAs (ctor));
    }
  }
}