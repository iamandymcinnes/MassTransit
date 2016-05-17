// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Policies
{
    using System;
    using System.Linq;
    using Util;


    public class PolicyExceptPolicyExceptionFilter :
        IPolicyExceptionFilter
    {
        readonly Type[] _exceptionTypes;

        public PolicyExceptPolicyExceptionFilter(params Type[] exceptionTypes)
        {
            _exceptionTypes = exceptionTypes;
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            ProbeContext scope = context.CreateScope("except");
            scope.Set(new
            {
                ExceptionTypes = _exceptionTypes.Select(TypeMetadataCache.GetShortName).ToArray(),
            });
        }

        public bool Match(Exception exception)
        {
            var baseException = exception.GetBaseException();

            for (int i = 0; i < _exceptionTypes.Length; i++)
            {
                if (_exceptionTypes[i].IsInstanceOfType(exception))
                    return false;

                if (_exceptionTypes[i].IsInstanceOfType(baseException))
                    return false;
            }

            return true;
        }
    }
}