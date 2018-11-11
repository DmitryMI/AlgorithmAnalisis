using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04.Sorting
{
    class TreeSorter : Sorter
    {

        protected override void Process(int[] collection)
        {
            result = new int[collection.Length];
            index = 0;
            result = collection;
            TreeSort(collection);

        }

        private int[] result;
        private int index = 0;


        class BinaryTree
        {
            public int Node;
            public bool Initialized;
            public BinaryTree LeftSubTree;
            public BinaryTree RightSubTree;
        }

        void Insert(BinaryTree searchTree, int item)
        {
            while (true)
            {
                if (!searchTree.Initialized)
                {
                    searchTree.Node = item;
                    searchTree.Initialized = true;
                }
                else
                {
                    BinaryTree side;
                    if (item < searchTree.Node)
                    {
                        if (searchTree.LeftSubTree == null)
                            searchTree.LeftSubTree = new BinaryTree();
                        side = searchTree.LeftSubTree;
                    }
                    else
                    {
                        if (searchTree.RightSubTree == null)
                            searchTree.RightSubTree = new BinaryTree();
                        side = searchTree.RightSubTree;
                    }
                    searchTree = side;
                    continue;
                }
                break;
            }
        }

        void Emit(int item)
        {
            result[index] = item;
            index++;
        }

        void InOrder(BinaryTree searchTree)
        {
            if (searchTree == null || !searchTree.Initialized)
            {
                return;
            }
            InOrder(searchTree.LeftSubTree);
            Emit((int)searchTree.Node);
            InOrder(searchTree.RightSubTree);
        }

        void TreeSort(int[] arr)
        {
            BinaryTree searchTree = new BinaryTree();

            for(int i = 0; i < arr.Length; i++)
            {
                Insert(searchTree, arr[i]);
            }

            InOrder(searchTree);
        }
    }
}
