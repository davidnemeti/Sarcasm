﻿#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

extern alias globalMiniPL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sarcasm.UnitTest
{
    [TestClass]
    public class GrammarTest : CommonTest
    {
        protected const string category = "GrammarTest";

        [TestMethod]
        [TestCategory(category)]
        public void GrammarCheck()
        {
            CommonTest.InitializeGrammar();
        }
    }
}
