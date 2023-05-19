using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace DataStructures
{
    class TextGenerationCacheNode
    {
        public string Key { get; private set; }
        private HashSet<TextGenerationCacheNode> _Children;
        public bool IsLeaf()
        {
            return _Children == null;
        }
        public HashSet<TextGenerationCacheNode> Children
        {
            get => _Children;
        }

        public TextGenerationCacheNode(string key)
        {
            Key = key;
            _Children = null;
        }
        public override bool Equals(object obj)
        {
            return Key.Equals(((TextGenerationCacheNode)obj).Key);
        }
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public void AddPropositions(string wordList)
        {
            var inputs = UtilityFunctions.Split(wordList);
            AddPropositionHelper(this, inputs, 0);
        }

        private void AddPropositionHelper(TextGenerationCacheNode root, string[] inputs, int position)
        {
            if (position >= inputs.Length)
            {
                return;
            }
            var newKey = inputs[position];
            var newChild = root.AddChild(newKey);
            AddPropositionHelper(newChild, inputs, ++position);
        }

        public List<string> GetPropositions(int maxWords, bool filter = false, string[] inputFilter = null, int filterIndex = 0)
        {
            var result = new List<string>();
            GetPropositionHelper(this, 0, maxWords, string.Empty, result, filter, inputFilter, filterIndex);
            result.Sort(delegate (string a, string b)
            {
                if (a == null) return -1;
                if (b == null) return 1;
                return a.Length >= b.Length ? 1 : -1;
            });
            return result;
        }

        private void GetPropositionHelper(TextGenerationCacheNode root, int pathLen, int maxWords, string currentPath, List<string> allPropositions,
            bool filter = false, string[] inputFilter = null, int filterIndex = 0)
        {
            if (pathLen == 1) currentPath = string.Empty;
            if (!filter || (filter && (filterIndex >= inputFilter.Length || root.Key.CompareTo(inputFilter[filterIndex]) == 0)))
            {
                currentPath = currentPath == string.Empty ? root.Key : currentPath + $" {root.Key}";
                ++pathLen;
                if (root.IsLeaf() || pathLen >= maxWords)
                {
                    allPropositions.Add(currentPath);
                }
                else
                {
                    foreach (var node in root.Children)
                    {
                        var tmp = $"{currentPath}";
                        GetPropositionHelper(node, pathLen, maxWords, tmp, allPropositions,
                            filter, inputFilter, ++filterIndex);
                    }
                }
            }
        }
        public TextGenerationCacheNode AddChild(string key)
        {
            var node = new TextGenerationCacheNode(key);
            if (_Children == null)
            {
                _Children = new HashSet<TextGenerationCacheNode>();
            }
            // Conservative, if node exists, nothing is done
            if (_Children.Add(node))
            {
                return node;
            }
            else
            {
                return _Children.First(n => n.Key == key);
            }
        }
    }
}
