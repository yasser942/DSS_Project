using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace DSS_LastAssignment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        OpenFileDialog ofd = new OpenFileDialog();
        String fileDirectory = "";
        double max = -9999999;
        int count = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Filter = "ARFF|*.arff";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.SafeFileName;
                fileDirectory = ofd.FileName;
            }
        }

        static weka.classifiers.Classifier cl_Naive = null;
        static weka.classifiers.Classifier cl_Knn = null;
        static weka.classifiers.Classifier cl_Tree = null;
        static weka.classifiers.Classifier cl_NN = null;
        static weka.classifiers.Classifier cl_SVM = null;
        const int percentSplit = 66;

        private void result_Click(object sender, EventArgs e)
        {
             List<string> algorithms = new List<string>
            {
                "Naive Bayes",
                "K Nearest Neighbor",
                "Decision Tree",
                "Neural Network",
                "Support Vector Machine"
            };

            List<double> successPercent = new List<double>();

            double res_Naive, res_KNN, res_NN, res_Tree, res_SVM = 0.0;
            string nameOfAlgo = "";

            //NAIVE BAYES ALGORITHM
            weka.core.Instances insts = new weka.core.Instances(new java.io.FileReader(fileDirectory));

            //CREATIING DYNAMIC GRIDVIEW FOR ADDING NEW INSTANCE
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowCount = insts.numAttributes();
            String[,] matrixOfInstances = new String[insts.numInstances(), insts.numAttributes()];



            for (int y = 0; y < insts.numAttributes() - 1; y++)
            {
                dataGridView1.Rows[y].Cells[0].Value = insts.attribute(y).name();
                if (insts.attribute(y).isNominal())
                {
                    //nominalDataValues.Add(insts.attribute(y).toString());
                    string phrase = insts.attribute(y).toString();
                    string[] first = phrase.Split('{');

                    string[] second = first[1].Split('}');

                    string[] attributeValues = second[0].Split(',');

                    DataGridViewComboBoxCell comboColumn = new DataGridViewComboBoxCell();

                    foreach (var a in attributeValues)
                    {
                        comboColumn.Items.Add(a);
                    }
                    dataGridView1.Rows[y].Cells[1] = comboColumn;
                }
            }

            insts.setClassIndex(insts.numAttributes() - 1);
            cl_Naive = new weka.classifiers.bayes.NaiveBayes();

            weka.filters.Filter myNominalData = new weka.filters.unsupervised.attribute.Discretize();
            myNominalData.setInputFormat(insts);
            insts = weka.filters.Filter.useFilter(insts, myNominalData);


            //randomize the order of the instances in the dataset.
            weka.filters.Filter myRandom = new weka.filters.unsupervised.instance.Randomize();
            myRandom.setInputFormat(insts);
            insts = weka.filters.Filter.useFilter(insts, myRandom);

            int trainSize = insts.numInstances() * percentSplit / 100;
            int testSize = insts.numInstances() - trainSize;
            weka.core.Instances train = new weka.core.Instances(insts, 0, trainSize);

            cl_Naive.buildClassifier(train);

            string str = cl_Naive.toString();

            int numCorrect = 0;
            for (int i = trainSize; i < insts.numInstances(); i++)
            {
                weka.core.Instance currentInst = insts.instance(i);
                double predictedClass = cl_Naive.classifyInstance(currentInst);
                if (predictedClass == insts.instance(i).classValue())
                    numCorrect++;
            }
            res_Naive = (double)((double)numCorrect / (double)testSize * 100.0);
            successPercent.Add(res_Naive);
            //kNN 

            weka.core.Instances insts2 = new weka.core.Instances(new java.io.FileReader(fileDirectory));

            insts2.setClassIndex(insts2.numAttributes() - 1);

            cl_Knn = new weka.classifiers.lazy.IBk();

            //Nominal to Binary
            weka.filters.Filter myBinaryData = new weka.filters.unsupervised.attribute.NominalToBinary();
            myBinaryData.setInputFormat(insts2);
            insts2 = weka.filters.Filter.useFilter(insts2, myBinaryData);

            //Normalization
            weka.filters.Filter myNormalized = new weka.filters.unsupervised.instance.Normalize();
            myNormalized.setInputFormat(insts2);
            insts2 = weka.filters.Filter.useFilter(insts2, myNormalized);

            //randomize the order of the instances in the dataset.
            weka.filters.Filter myRandom2 = new weka.filters.unsupervised.instance.Randomize();
            myRandom2.setInputFormat(insts2);
            insts2 = weka.filters.Filter.useFilter(insts2, myRandom2);

            int trainSize2 = insts2.numInstances() * percentSplit / 100;
            int testSize2 = insts2.numInstances() - trainSize2;
            weka.core.Instances train2 = new weka.core.Instances(insts2, 0, trainSize2);

            cl_Knn.buildClassifier(train2);

            string str2 = cl_Knn.toString();

            int numCorrect2 = 0;
            for (int i = trainSize2; i < insts2.numInstances(); i++)
            {
                weka.core.Instance currentInst2 = insts2.instance(i);
                double predictedClass = cl_Knn.classifyInstance(currentInst2);
                if (predictedClass == insts2.instance(i).classValue())
                    numCorrect2++;
            }
            res_KNN = (double)((double)numCorrect2 / (double)testSize2 * 100.0);
            successPercent.Add(res_KNN);

            //Decision tree
            weka.core.Instances insts3 = new weka.core.Instances(new java.io.FileReader(fileDirectory));

            insts3.setClassIndex(insts3.numAttributes() - 1);

            cl_Tree = new weka.classifiers.trees.J48();



            weka.filters.Filter myNormalized2 = new weka.filters.unsupervised.instance.Normalize();
            myNormalized2.setInputFormat(insts3);
            insts3 = weka.filters.Filter.useFilter(insts3, myNormalized2);


            //randomize the order of the instances in the dataset.
            weka.filters.Filter myRandom3 = new weka.filters.unsupervised.instance.Randomize();
            myRandom3.setInputFormat(insts3);
            insts3 = weka.filters.Filter.useFilter(insts3, myRandom3);

            int trainSize3 = insts3.numInstances() * percentSplit / 100;
            int testSize3 = insts3.numInstances() - trainSize3;
            weka.core.Instances train3 = new weka.core.Instances(insts3, 0, trainSize3);

            cl_Tree.buildClassifier(train3);

            string str3 = cl_Tree.toString();

            int numCorrect3 = 0;
            for (int i = trainSize3; i < insts3.numInstances(); i++)
            {
                weka.core.Instance currentInst3 = insts3.instance(i);
                double predictedClass = cl_Tree.classifyInstance(currentInst3);
                if (predictedClass == insts3.instance(i).classValue())
                    numCorrect3++;
            }
            res_Tree = (double)((double)numCorrect3 / (double)testSize3 * 100.0);
            successPercent.Add(res_Tree);

            //Neural Network
            weka.core.Instances insts4 = new weka.core.Instances(new java.io.FileReader(fileDirectory));

            insts4.setClassIndex(insts4.numAttributes() - 1);

            cl_NN = new weka.classifiers.functions.MultilayerPerceptron();

            //Nominal to Binary
            weka.filters.Filter myBinaryData2 = new weka.filters.unsupervised.attribute.NominalToBinary();
            myBinaryData2.setInputFormat(insts4);
            insts4 = weka.filters.Filter.useFilter(insts4, myBinaryData2);

            //Normalization
            weka.filters.Filter myNormalized3 = new weka.filters.unsupervised.instance.Normalize();
            myNormalized3.setInputFormat(insts4);
            insts4 = weka.filters.Filter.useFilter(insts4, myNormalized3);

            //randomize the order of the instances in the dataset.
            weka.filters.Filter myRandom4 = new weka.filters.unsupervised.instance.Randomize();
            myRandom4.setInputFormat(insts4);
            insts4 = weka.filters.Filter.useFilter(insts4, myRandom4);

            int trainSize4 = insts4.numInstances() * percentSplit / 100;
            int testSize4 = insts4.numInstances() - trainSize4;
            weka.core.Instances train4 = new weka.core.Instances(insts4, 0, trainSize4);

            cl_NN.buildClassifier(train4);

            string str4 = cl_NN.toString();

            int numCorrect4 = 0;
            for (int i = trainSize4; i < insts4.numInstances(); i++)
            {
                weka.core.Instance currentInst4 = insts4.instance(i);
                double predictedClass = cl_NN.classifyInstance(currentInst4);
                if (predictedClass == insts4.instance(i).classValue())
                    numCorrect4++;
            }

            res_NN = (double)((double)numCorrect4 / (double)testSize4 * 100.0);
            successPercent.Add(res_NN);

            //SVM
            weka.core.Instances insts5 = new weka.core.Instances(new java.io.FileReader(fileDirectory));

            insts5.setClassIndex(insts5.numAttributes() - 1);

            cl_SVM = new weka.classifiers.functions.SMO();

            //Nominal to Binary
            weka.filters.Filter myBinaryData3 = new weka.filters.unsupervised.attribute.NominalToBinary();
            myBinaryData3.setInputFormat(insts5);
            insts5 = weka.filters.Filter.useFilter(insts5, myBinaryData3);

            //Normalization
            weka.filters.Filter myNormalized4 = new weka.filters.unsupervised.instance.Normalize();
            myNormalized4.setInputFormat(insts5);
            insts5 = weka.filters.Filter.useFilter(insts5, myNormalized4);

            //randomize the order of the instances in the dataset.
            weka.filters.Filter myRandom5 = new weka.filters.unsupervised.instance.Randomize();
            myRandom5.setInputFormat(insts5);
            insts5 = weka.filters.Filter.useFilter(insts5, myRandom5);

            int trainSize5 = insts5.numInstances() * percentSplit / 100;
            int testSize5 = insts5.numInstances() - trainSize5;
            weka.core.Instances train5 = new weka.core.Instances(insts5, 0, trainSize5);

            cl_SVM.buildClassifier(train5);

            string str5 = cl_SVM.toString();

            int numCorrect5 = 0;
            for (int i = trainSize5; i < insts5.numInstances(); i++)
            {
                weka.core.Instance currentInst5 = insts5.instance(i);
                double predictedClass = cl_SVM.classifyInstance(currentInst5);
                if (predictedClass == insts5.instance(i).classValue())
                    numCorrect5++;
            }
            res_SVM = (double)((double)numCorrect5 / (double)testSize5 * 100.0);
            successPercent.Add(res_SVM);


            for (int i = 0; i < successPercent.Count; i++)
            {

                if ((double)successPercent[i] > max)
                {
                    max = (double)successPercent[i];
                    count = i + 1;
                }

            }
            for (int i = 0; i < count; i++)
            {
                nameOfAlgo = (string)algorithms[i];
            }

            label1.Text = nameOfAlgo + " is the most successful algorithm for this data set." + "(" + max + "%)\n";


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Discover_Click(object sender, EventArgs e)
        {
            String newDirectory = "test.arff"; // for algortihms
            // Clear the contents of the "test.arff" file
            File.WriteAllText(newDirectory, string.Empty);

            // Open the file in write mode to append new data
            using (StreamWriter sw = new StreamWriter(newDirectory, true))
            {
                // Read data from the specified file until encountering "@data" or "@DATA"
                using (StreamReader sr = new StreamReader(fileDirectory))
                {
                    string line;
                    string comp = "@data";
                    string comp2 = "@DATA";

                    while ((line = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                        if (line == comp || line == comp2)
                            break;
                    }
                }

                // Construct a new instance based on user input from the DataGridView
                String s_newInstance = "";
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    s_newInstance += (String)dataGridView1.Rows[i].Cells[1].Value + ",";
                }
                s_newInstance += "?";
                sw.WriteLine(s_newInstance);
            }


            switch (count)
            {
                case 1:
                    weka.core.Instances insts = new weka.core.Instances(new java.io.FileReader(newDirectory));
                    insts.setClassIndex(insts.numAttributes() - 1);

                    weka.filters.Filter myNominalData = new weka.filters.unsupervised.attribute.Discretize();
                    myNominalData.setInputFormat(insts);
                    insts = weka.filters.Filter.useFilter(insts, myNominalData);

                    //randomize the order of the instances in the dataset.
                    weka.filters.Filter myRandom = new weka.filters.unsupervised.instance.Randomize();
                    myRandom.setInputFormat(insts);
                    insts = weka.filters.Filter.useFilter(insts, myRandom);

                    double predictedClass = cl_Naive.classifyInstance(insts.instance(0));
                    Console.WriteLine("hey",insts.instance(0));
                    label2.Text = insts.classAttribute().value(Convert.ToInt32(predictedClass));

                    break;
                case 2:
                    weka.core.Instances insts2 = new weka.core.Instances(new java.io.FileReader(fileDirectory));

                    insts2.setClassIndex(insts2.numAttributes() - 1);

                    //Nominal to Binary
                    weka.filters.Filter myBinaryData = new weka.filters.unsupervised.attribute.NominalToBinary();
                    myBinaryData.setInputFormat(insts2);
                    insts2 = weka.filters.Filter.useFilter(insts2, myBinaryData);

                    //Normalization
                    weka.filters.Filter myNormalized = new weka.filters.unsupervised.instance.Normalize();
                    myNormalized.setInputFormat(insts2);
                    insts2 = weka.filters.Filter.useFilter(insts2, myNormalized);

                    //randomize the order of the instances in the dataset.
                    weka.filters.Filter myRandom2 = new weka.filters.unsupervised.instance.Randomize();
                    myRandom2.setInputFormat(insts2);
                    insts2 = weka.filters.Filter.useFilter(insts2, myRandom2);

                    double predictedClass2 = cl_Knn.classifyInstance(insts2.instance(0));
                    label2.Text = insts2.classAttribute().value(Convert.ToInt32(predictedClass2));
                    break;
                case 3:
                    weka.core.Instances insts3 = new weka.core.Instances(new java.io.FileReader(newDirectory));

                    insts3.setClassIndex(insts3.numAttributes() - 1);
                    weka.filters.Filter myNormalized2 = new weka.filters.unsupervised.instance.Normalize();
                    myNormalized2.setInputFormat(insts3);
                    insts3 = weka.filters.Filter.useFilter(insts3, myNormalized2);


                    //randomize the order of the instances in the dataset.
                    weka.filters.Filter myRandom3 = new weka.filters.unsupervised.instance.Randomize();
                    myRandom3.setInputFormat(insts3);
                    insts3 = weka.filters.Filter.useFilter(insts3, myRandom3);

                    double predictedClass3 = cl_Tree.classifyInstance(insts3.instance(0));
                    label2.Text = insts3.classAttribute().value(Convert.ToInt32(predictedClass3));
                    break;
                case 4:
                    weka.core.Instances insts4 = new weka.core.Instances(new java.io.FileReader(newDirectory));
                    insts4.setClassIndex(insts4.numAttributes() - 1);
                    //cl = new weka.classifiers.functions.MultilayerPerceptron();

                    //Nominal to Binary
                    weka.filters.Filter myBinaryData2 = new weka.filters.unsupervised.attribute.NominalToBinary();
                    myBinaryData2.setInputFormat(insts4);
                    insts4 = weka.filters.Filter.useFilter(insts4, myBinaryData2);

                    //Normalization
                    weka.filters.Filter myNormalized3 = new weka.filters.unsupervised.instance.Normalize();
                    myNormalized3.setInputFormat(insts4);
                    insts4 = weka.filters.Filter.useFilter(insts4, myNormalized3);

                    //randomize the order of the instances in the dataset.
                    weka.filters.Filter myRandom4 = new weka.filters.unsupervised.instance.Randomize();
                    myRandom4.setInputFormat(insts4);
                    insts4 = weka.filters.Filter.useFilter(insts4, myRandom4);

                    double predictedClass4 = cl_NN.classifyInstance(insts4.instance(0));
                    label2.Text = insts4.classAttribute().value(Convert.ToInt32(predictedClass4));

                    break;
                case 5:
                    weka.core.Instances insts5 = new weka.core.Instances(new java.io.FileReader(newDirectory));

                    insts5.setClassIndex(insts5.numAttributes() - 1);


                    //Nominal to Binary
                    weka.filters.Filter myBinaryData3 = new weka.filters.unsupervised.attribute.NominalToBinary();
                    myBinaryData3.setInputFormat(insts5);
                    insts5 = weka.filters.Filter.useFilter(insts5, myBinaryData3);

                    //Normalization
                    weka.filters.Filter myNormalized4 = new weka.filters.unsupervised.instance.Normalize();
                    myNormalized4.setInputFormat(insts5);
                    insts5 = weka.filters.Filter.useFilter(insts5, myNormalized4);

                    //randomize the order of the instances in the dataset.
                    weka.filters.Filter myRandom5 = new weka.filters.unsupervised.instance.Randomize();
                    myRandom5.setInputFormat(insts5);
                    insts5 = weka.filters.Filter.useFilter(insts5, myRandom5);

                    double predictedClass5 = cl_SVM.classifyInstance(insts5.instance(0));
                    label2.Text = insts5.classAttribute().value(Convert.ToInt32(predictedClass5));
                    break;

                default:
                    label2.Text = "Error!";
                    break;


            }



        }
    }
}

