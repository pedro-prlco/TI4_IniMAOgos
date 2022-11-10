using System.Collections.Generic;
using UnityEngine;

using TI4;

namespace TI4.IA
{

    /*
    
    tipo de fase (dificuldade) - range quantidade de inimigos
    Fase tipo 1: Tela: 0 a 2; Inimigos: 5 a 8 inimigos; Dificuldade: 0,1.;
    Fase tipo 2: Tela: 0 a 3; Inimigos: 7 a 10 inimigos; Dificuldade: 0,1,2.; 
    Fase tipo 3: Tela: 0 a 4; Inimigos: 9 a 12 inimigos; Dificuldade: 0,1,2.; 
    Fase tipo 4: Tela: 0 a 5; Inimigos: 11 a 14 inimigos; Dificuldade: 0,1,2,3.; 
    Fase tipo 5: Tela: 0 a 5; Inimigos: 15 a 20 inimigos; Dificuldade: 0,1,2,3.; 

    */

    public class DecisionTreeManager : MonoBehaviour
    {
        double[][] dataX;
        int[] dataY;
        DecisionTree dt;

        public static DecisionTreeManager instance;

        void Awake()
        {
            instance = this;
        }

        public void Setup(out int level)
        {
            dataX = new double[52][];
            dataX[0] = new double[] { 1, 0, 0, 8 };  // 0
            dataX[1] = new double[] { 2, 0, 0, 10 };
            dataX[2] = new double[] { 3, 0, 0, 12 };
            dataX[3] = new double[] { 4, 0, 0, 14 };
            dataX[4] = new double[] { 5, 0, 0, 20 };
            dataX[5] = new double[] { 1, 5, 1, 3 };
            dataX[6] = new double[] { 2, 5, 1, 5 };
            dataX[7] = new double[] { 3, 5, 2, 7 };
            dataX[8] = new double[] { 4, 5, 3, 9 };
            dataX[9] = new double[] { 5, 5, 2, 15 };

            dataX[10] = new double[] { 1, 3, 1, 0 };  // 1
            dataX[11] = new double[] { 2, 2, 2, 0 };
            dataX[12] = new double[] { 3, 4, 2, 0 };
            dataX[13] = new double[] { 4, 1, 2, 0 };
            dataX[14] = new double[] { 5, 5, 3, 0 };
            dataX[15] = new double[] { 1, 2, 1, 4 };
            dataX[16] = new double[] { 2, 3, 2, 6 };
            dataX[17] = new double[] { 3, 4, 2, 8 };
            dataX[18] = new double[] { 4, 3, 3, 9 };
            dataX[19] = new double[] { 5, 4, 3, 12 };

            dataX[20] = new double[] { 1, 4, 1, 3 };   // 2
            dataX[21] = new double[] { 2, 1, 1, 8 };
            dataX[22] = new double[] { 3, 2, 1, 6 };
            dataX[23] = new double[] { 4, 4, 2, 2 };
            dataX[24] = new double[] { 5, 4, 3, 9 };
            dataX[25] = new double[] { 4, 1, 1, 5 };
            dataX[26] = new double[] { 5, 3, 3, 10 };
            dataX[27] = new double[] { 4, 2, 1, 10 };
            dataX[28] = new double[] { 5, 2, 2, 5 };
            dataX[29] = new double[] { 1, 1, 1, 5 };

            dataX[30] = new double[] { 1, 3, 1, 1 };   // 2
            dataX[31] = new double[] { 1, 4, 1, 8 };
            dataX[32] = new double[] { 2, 3, 2, 5 };
            dataX[33] = new double[] { 2, 4, 2, 3 };
            dataX[34] = new double[] { 2, 2, 1, 8 };
            dataX[35] = new double[] { 4, 4, 3, 10 };
            dataX[36] = new double[] { 1, 5, 1, 2 };
            dataX[37] = new double[] { 2, 1, 2, 0 };
            dataX[38] = new double[] { 3, 5, 2, 3 };
            dataX[39] = new double[] { 4, 1, 2, 0 };

            dataX[40] = new double[] { 5, 5, 2, 10 };   // 2
            dataX[41] = new double[] { 3, 2, 1, 3 };
            dataX[42] = new double[] { 3, 1, 2, 2 };
            dataX[43] = new double[] { 2, 0, 0, 5 };
            dataX[44] = new double[] { 5, 4, 3, 10 };
            dataX[45] = new double[] { 5, 4, 2, 7 };
            dataX[46] = new double[] { 5, 4, 2, 3 };
            dataX[47] = new double[] { 4, 4, 1, 1 };
            dataX[48] = new double[] { 4, 3, 1, 10 };
            dataX[49] = new double[] { 4, 2, 2, 5 };
            dataX[50] = new double[] { 2, 2, 1, 3 };
            dataX[51] = new double[] { 4, 4, 2.75d, 8 };

            dataY =
              new int[] { 1, 1, 2, 3, 3, 0, 0, 0, 0, 0,
                      0, 0, 0, 0, 0, 1, 1, 1, 1, 1,
                      1, 2, 2, 2, 2, 3, 3, 3, 3, 1,
                      1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                      0, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                      2, 1 };

            dt = new DecisionTree(250, 4);
            dt.BuildTree(dataX, dataY);

            // 
            // dt.Show();  // show all nodes in tree

            dt.ShowNode(0);
            dt.ShowNode(4);

            double acc = dt.Accuracy(dataX, dataY);

            //Variáveis a serem alteradas
            double fase = LevelInfo.currentData.Id;
            double tela = EnemySpawner.EnemiesInScreen.Count;
            

            int totalSum = 0;
            EnemySpawner.EnemiesInScreen.ForEach(enemy => 
            {
                totalSum += enemy.Dificuldade;
            });

            double dificuldade =  totalSum / Mathf.Max(EnemySpawner.EnemiesInScreen.Count, 1);
            double restante = LevelInfo.currentData.MaxEnemies - EnemySpawner.TotalSpawned;

            double[] x = new double[] { fase, tela, dificuldade, restante };

            int predClass = dt.Predict(x, verbose: true);
            level = predClass;
        }
    }

    class DecisionTree
    {
        public int numNodes;
        public int numClasses;
        public List<Node> tree;

        public DecisionTree(int numNodes, int numClasses)
        {
            this.numNodes = numNodes;
            this.numClasses = numClasses;
            this.tree = new List<Node>();
            for (int i = 0; i < numNodes; ++i)
                this.tree.Add(new Node());
        }

        public void BuildTree(double[][] dataX, int[] dataY)
        {
            // prep the list and the root node
            int n = dataX.Length;

            List<int> allRows = new List<int>();
            for (int i = 0; i < n; ++i)
                allRows.Add(i);

            this.tree[0].rows = new List<int>(allRows);

            for (int i = 0; i < this.numNodes; ++i)  // finish root, and do remaining
            {
                this.tree[i].nodeID = i;

                SplitInfo si = GetSplitInfo(dataX, dataY, this.tree[i].rows, this.numClasses);  // get the split info
                tree[i].splitCol = si.splitCol;
                tree[i].splitVal = si.splitVal;

                tree[i].classCounts = ComputeClassCts(dataY, this.tree[i].rows, this.numClasses);
                tree[i].predictedClass = ArgMax(tree[i].classCounts);

                int leftChild = (2 * i) + 1;
                int rightChild = (2 * i) + 2;

                if (leftChild < numNodes)
                    tree[leftChild].rows = new List<int>(si.lessRows);
                if (rightChild < numNodes)
                    tree[rightChild].rows = new List<int>(si.greaterRows);
            } // i

        } // BuildTree()

        public void Show()
        {
            for (int i = 0; i < this.numNodes; ++i)
                ShowNode(i);
        }

        public void ShowNode(int nodeID)
        {
            Debug.Log("Node class counts: ");
            for (int c = 0; c < this.numClasses; ++c)
                Debug.Log(this.tree[nodeID].classCounts[c] + "  ");
        }

        //ESSE AQUI !
        public int Predict(double[] x, bool verbose)
        {
            bool vb = verbose;
            int result = -1;
            int currNodeID = 0;
            string rule = "IF (*)";  // if any column is any value . . 
            while (true)
            {
                if (this.tree[currNodeID].rows.Count == 0)  // at an empty node
                    break;

                int sc = this.tree[currNodeID].splitCol;
                double sv = this.tree[currNodeID].splitVal;
                double v = x[sc];

                if (v < sv)
                {
                    currNodeID = (2 * currNodeID) + 1;
                    if (currNodeID >= this.tree.Count)
                        break;
                    result = this.tree[currNodeID].predictedClass;
                    rule += " AND (column " + sc + " < " + sv + ")";
                }
                else
                {
                    currNodeID = (2 * currNodeID) + 2;
                    if (currNodeID >= this.tree.Count)
                        break;
                    result = this.tree[currNodeID].predictedClass;
                    rule += " AND (column " + sc + " >= " + sv + ")";
                }

            }

            return result;
        } // Prediction

        public double Accuracy(double[][] dataX, int[] dataY)
        {
            int numCorrect = 0;
            int numWrong = 0;
            for (int i = 0; i < dataX.Length; ++i)
            {
                int predicted = Predict(dataX[i], verbose: false);
                int actual = dataY[i];
                if (predicted == actual)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            // 
            // 
            return (numCorrect * 1.0) / (numWrong + numCorrect);
        }

        private static SplitInfo GetSplitInfo(double[][] dataX, int[] dataY, List<int> rows, int numClasses)
        {
            // given a set of parent rows, find the col and value, and less-rows and greater-rows of
            // partition that gives lowest resulting mean impurity or entropy
            int nCols = dataX[0].Length;
            SplitInfo result = new SplitInfo();

            int bestSplitCol = 0;
            double bestSplitVal = 0.0;
            double bestImpurity = double.MaxValue;
            List<int> bestLessRows = new List<int>();
            List<int> bestGreaterRows = new List<int>();  // actually >=

            foreach (int i in rows)  // traverse the specified rows of the ref data
            {
                for (int j = 0; j < nCols; ++j)
                {
                    double splitVal = dataX[i][j];  // curr value to evaluate as possible best split value
                    List<int> lessRows = new List<int>();
                    List<int> greaterRows = new List<int>();
                    foreach (int ii in rows)  // walk down curr column
                    {
                        if (dataX[ii][j] < splitVal)
                            lessRows.Add(ii);
                        else
                            greaterRows.Add(ii);
                    } // ii

                    double meanImp = MeanImpurity(dataY, lessRows, greaterRows, numClasses);

                    if (meanImp < bestImpurity)
                    {
                        bestImpurity = meanImp;
                        bestSplitCol = j;
                        bestSplitVal = splitVal;

                        bestLessRows = new List<int>(lessRows);  // could use a CopyOf() helper
                        bestGreaterRows = new List<int>(greaterRows);
                    }

                } // j
            } // i

            result.splitCol = bestSplitCol;
            result.splitVal = bestSplitVal;
            result.lessRows = new List<int>(bestLessRows);
            result.greaterRows = new List<int>(bestGreaterRows);

            return result;
        }

        private static double Impurity(int[] dataY, List<int> rows, int numClasses)
        {
            // Gini impurity
            // dataY is all Y (class) values; rows tells which ones to analyze

            if (rows.Count == 0) return 0.0;

            int[] counts = new int[numClasses];  // counts for each of the classes
            double[] probs = new double[numClasses];  // frequency each class
            for (int i = 0; i < rows.Count; ++i)
            {
                int idx = rows[i];  // pts into refY
                int c = dataY[idx];  // class
                ++counts[c];
            }

            for (int c = 0; c < numClasses; ++c)
                if (counts[c] == 0) probs[c] = 0.0;
                else probs[c] = (counts[c] * 1.0) / rows.Count;

            double sum = 0.0;
            for (int c = 0; c < numClasses; ++c)
                sum += probs[c] * probs[c];

            return 1.0 - sum;
        }

        private static double MeanImpurity(int[] dataY, List<int> rows1, List<int> rows2, int numClasses)
        {
            if (rows1.Count == 0 && rows2.Count == 0)
                return 0.0;

            double imp1 = Impurity(dataY, rows1, numClasses); // 0.0 if rows Count is 0
            double imp2 = Impurity(dataY, rows2, numClasses);
            int count1 = rows1.Count;
            int count2 = rows2.Count;
            double wt1 = (count1 * 1.0) / (count1 + count2);
            double wt2 = (count2 * 1.0) / (count1 + count2);
            double result = (wt1 * imp1) + (wt2 * imp2);
            return result;
        }

        private static int[] ComputeClassCts(int[] dataY, List<int> rows, int numClasses)
        {
            int[] result = new int[numClasses];
            foreach (int i in rows)
            {
                int c = dataY[i];
                ++result[c];
            }
            return result;
        }

        private static int ArgMax(int[] classCts)
        {
            int largeCt = 0;
            int largeIndx = 0;
            for (int i = 0; i < classCts.Length; ++i)
            {
                if (classCts[i] > largeCt)
                {
                    largeCt = classCts[i];
                    largeIndx = i;
                }
            }
            return largeIndx;
        }

        //private static List<int> CopyOf(List<int> rows)
        //{
        //  List<int> result = new List<int>();
        //  foreach (int i in rows)
        //    result.Add(i);
        //  return result;
        //}

        // ----------

        public class Node
        {
            public int nodeID;
            public List<int> rows;  // which ref data rows
            public int splitCol;
            public double splitVal;
            public int[] classCounts;
            public int predictedClass;
        }

        public class SplitInfo  // helper struc
        {
            public int splitCol;
            public double splitVal;
            public List<int> lessRows;
            public List<int> greaterRows;
        }
    }
}