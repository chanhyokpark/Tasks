using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tasks_wpf
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        List<object> lst = new List<object>();
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("읽어주세요:\n 이 프로그램은 chrome 111을 요구합니다. \n 환경에 따라 실행되지 않거나 오류가 발생할 수 있습니다. \n 만약 chrome 버전이 맞지 않으면 버전에 맞는 chromedriver.exe를 설치하여 대체하십시오. \n처음 실행시에는 chatgpt plus가 적용된 계정으로 로그인이 필요합니다. (같이 포함된 gpt.exe에서 로그인 항목을 선택하십시오.)");

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GridSplitter_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            
            
                        
        }

        private void T1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                T2.Focus();
            }
        }

        private void T2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                T3.Focus();
            }
        }

        private void T3_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                lst.Add(new { name = T1.Text, duration = T2.Text, neededtime = T3.Text});
                TaskList.Items.Add(new { name = T1.Text, duration = T2.Text, neededtime = T3.Text });
                T1.Text = "";
                T2.Text = "";
                T3.Text = "";
                T1.Focus();
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            LoadingForm lf = new LoadingForm();
            lf.Show();
            lf.StartGen(lst);
        }

        private void T3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
