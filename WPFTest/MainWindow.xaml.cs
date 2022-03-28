using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFTest
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static int countQ;
        StackPanel[] Panels;
        Object[,] Question;
        int[] answers;
        int[] userAnswers;

        private void Window_Initialized(object sender, EventArgs e)
        {
            InitialMethod();
        }

        private void InitialMethod()
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(@"E:\KFU\WPFTest\WPFTest\myTest.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
            Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

            countQ = excelRange.Rows.Count - 1;

            Panels = new StackPanel[countQ];
            Question = new object[countQ, 4];

            answers = new int[countQ];
            userAnswers = new int[countQ];

            for (int i = 0; i < countQ; i++)
            {
                Panels[i] = new StackPanel();
                Panels[i].Orientation = Orientation.Vertical;
                Panels[i].Background = new SolidColorBrush(Colors.CadetBlue);
            }
            for (int i = 0; i < countQ; i++)
            {
                Question[i, 0] = new Label();
                (Question[i, 0] as Label).Content = Convert.ToString((excelRange.Cells[i + 2, 1] as Microsoft.Office.Interop.Excel.Range).Value2);
                (Question[i, 0] as Label).FontSize = 20;
                Panels[i].Children.Add(Question[i, 0] as Label);

                for (int j = 1; j < 4; j++)
                {
                    Question[i, j] = new RadioButton();
                    (Question[i, j] as RadioButton).Content = Convert.ToString((excelRange.Cells[i + 2, j + 1] as Microsoft.Office.Interop.Excel.Range).Value2);
                    (Question[i, j] as RadioButton).FontSize = 20;
                    Panels[i].Children.Add(Question[i, j] as RadioButton);
                }
                answers[i] = Convert.ToInt32((excelRange.Cells[i + 2, 5] as Microsoft.Office.Interop.Excel.Range).Value2);
                Panels[i].Margin = new Thickness(3, 5, 0, 0);
                MainPanel.Children.Add(Panels[i]);
            }

            Button btn = new Button();
            btn.Content = "Ok";
            btn.Width = 85;
            btn.Margin = new Thickness(10, 10, 10, 10);
            btn.HorizontalAlignment = HorizontalAlignment.Right;
            btn.Click += Btn_Click;
            MainPanel.Children.Add(btn);
            excelBook.Close(true, null, null);
            excelApp.Quit();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            BeforeCheckedMethod();
            CheckedMethod();
        }

        private void BeforeCheckedMethod()
        {
            for (int i = 0; i < countQ; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if ((Question[i, j] as RadioButton).IsChecked == true)
                    {
                        userAnswers[i] = j;
                    }
                }
            }


            for (int i = 0; i < countQ; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    (Question[i, j] as RadioButton).IsEnabled = false;
                }
            }
        }

        private void CheckedMethod()
        {
            int k = 0;
            for (int i = 0; i < countQ; i++)
            {
                if (userAnswers[i] != 0)
                    if (userAnswers[i] == answers[i])
                    {
                        (Question[i, userAnswers[i]] as RadioButton).Foreground = new SolidColorBrush(Colors.Green);
                        k++;
                    }
                    else
                    {
                        (Question[i, userAnswers[i]] as RadioButton).Foreground = new SolidColorBrush(Colors.Red);
                    }
                else
                {
                    (Question[i, answers[i]] as RadioButton).Foreground = new SolidColorBrush(Colors.Green);
                }
            }
            MessageBox.Show(k.ToString());
        }
    }
}
