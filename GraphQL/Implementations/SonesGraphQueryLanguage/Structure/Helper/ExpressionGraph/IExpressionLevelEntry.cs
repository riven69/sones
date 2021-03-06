/*
* sones GraphDB - Community Edition - http://www.sones.com
* Copyright (C) 2007-2011 sones GmbH
*
* This file is part of sones GraphDB Community Edition.
*
* sones GraphDB is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, version 3 of the License.
* 
* sones GraphDB is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with sones GraphDB. If not, see <http://www.gnu.org/licenses/>.
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sones.Library.Commons.VertexStore.Definitions;

namespace sones.GraphQL.GQL.Structure.Helper.ExpressionGraph
{
    public interface IExpressionLevelEntry
    {
        /// <summary>
        /// All Objects with their ExpressionNode
        /// VertexTypeID/VertexID --> node
        /// </summary>
        Dictionary<VertexInformation, IExpressionNode> Nodes { get; }

        /// <summary>
        /// The corresponding Level
        /// </summary>
        LevelKey CorrespondingLevelKey { get; }
    }
}
