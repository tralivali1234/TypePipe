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
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Remotion.TypePipe.MutableReflection;

namespace Remotion.TypePipe.UnitTests.MutableReflection
{
  [TestFixture]
  public class FutureMethodInfoTest
  {
    [Test]
    public void Initialization ()
    {
      var parameterDeclarations =
          new[]
          {
              new ParameterDeclaration (typeof (int), "p1", ParameterAttributes.Out),
              new ParameterDeclaration (typeof (string), "p2")
          };
      var methodInfo = new FutureMethodInfo (typeof (object), MethodAttributes.Public, parameterDeclarations);

      Assert.That (methodInfo.DeclaringType, Is.EqualTo (typeof (object)));
      Assert.That (methodInfo.Attributes, Is.EqualTo (MethodAttributes.Public));
      var expectedParameterInfos =
          new[]
          {
              new { Member = (MemberInfo) methodInfo, Position = 0, ParameterType = typeof (int), Name = "p1", Attributes = ParameterAttributes.Out },
              new { Member = (MemberInfo) methodInfo, Position = 1, ParameterType = typeof (string), Name = "p2", Attributes = ParameterAttributes.In }
          };
      var actualParameterInfos = methodInfo.GetParameters ().Select (pi => new { pi.Member, pi.Position, pi.ParameterType, pi.Name, pi.Attributes });
      Assert.That (actualParameterInfos, Is.EqualTo (expectedParameterInfos));
    }

    [Test]
    public void GetParameters_DoesNotAllowModificationOfInternalList ()
    {
      var ctorInfo = FutureMethodInfoObjectMother.Create (parameterDeclarations: new[] { ParameterDeclarationObjectMother.Create () });

      var parameters = ctorInfo.GetParameters ();
      Assert.That (parameters[0], Is.Not.Null);
      parameters[0] = null;

      var parametersAgain = ctorInfo.GetParameters ();
      Assert.That (parametersAgain[0], Is.Not.Null);
    }
  }
}