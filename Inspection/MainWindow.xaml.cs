using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Inspection
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapterTests;
        SqlDataAdapter adapterParams;
        DataSet testsTable;
        DataSet paramsTable;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["DBInsConnectionString"].ConnectionString;
        }

        private void DownloadTests()
        {
            string sql = "SELECT * FROM Tests";
            testsTable = new DataSet();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapterTests = new SqlDataAdapter(command);

                connection.Open();
                adapterTests.Fill(testsTable);
                dg_tests.ItemsSource = testsTable.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
        private void DownloadParams()
        {
            string sql = "SELECT * FROM Parameters";
            paramsTable = new DataSet();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapterParams = new SqlDataAdapter(command);

                connection.Open();
                adapterParams.Fill(paramsTable);
                dg_params.ItemsSource = paramsTable.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void dg_tests_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadTests();
        }
        private void dg_params_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadParams();
        }

        private void UpdateTests()
        {
            try
            {
                SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapterTests);
                adapterTests.Update(testsTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateParams()
        {
            try
            {
                SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapterParams);
                adapterParams.Update(paramsTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dg_tests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dg_tests.SelectedItem != null)
            {
                butt_delTest.IsEnabled = true;
            }
        }
        private void dg_params_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dg_params.SelectedItem != null)
            {
                butt_delParam.IsEnabled = true;
            }
        }

        private void butt_delTest_Click(object sender, RoutedEventArgs e)
        {
            if (dg_tests.SelectedItem != null)
            {
                for (int i = 0; i < dg_tests.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = dg_tests.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            if(dg_tests.SelectedItem == null)
                butt_delTest.IsEnabled = false;
        }
        private void butt_delParam_Click(object sender, RoutedEventArgs e)
        {
            if (dg_params.SelectedItem != null)
            {
                for (int i = 0; i < dg_params.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = dg_params.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            if (dg_params.SelectedItem == null)
                butt_delParam.IsEnabled = false;
        }

        private void butt_saveTest_Click(object sender, RoutedEventArgs e)
        {
            UpdateTests();
        }
        private void butt_saveParam_Click(object sender, RoutedEventArgs e)
        {
            UpdateParams();
        }
    }
}
