using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DataStructures
{
    public class TextGenerationCacheTree
    {
        TextGenerationCacheNode _Root;
        public TextGenerationCacheTree()
        {
            _Root = null;
        }
        public void AddPropositions(string wordList)
        {
            if(_Root == null)
            {
                _Root = new TextGenerationCacheNode("Root");
            }
            _Root.AddPropositions(wordList);
        }

        public List<string> GetPropositions(int maxWords, bool filter = false, string inputFilterStr = null)
        {
            if(!filter)
                return _Root.GetPropositions(++maxWords);
            else
            {
                var inputFilterTmp = UtilityFunctions.Split(inputFilterStr);
                var inputFilter = new string[inputFilterTmp.Length + 1];
                inputFilter[0] = "Root";
                inputFilterTmp.CopyTo(inputFilter, 1);
                return _Root.GetPropositions(++maxWords, filter, inputFilter, 0);
            }
        }
    }
}
