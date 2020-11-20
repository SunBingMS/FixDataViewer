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

namespace FixDataViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// btnShow_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SubGetString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void SubGetString()
        {
            try
            {
                int intStartStr = 0;
                int intLongStr = 0;
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");

                string[] strTmp = txtSubstring.Text.Split(',');
                if (strTmp.Length != 2) return;

                string strStartStr = strTmp[0].Trim();
                string strLongStr = strTmp[1].Trim();


                if (!int.TryParse(strStartStr, out intStartStr))
                {
                    txtResult1.Text = "";
                    txtResult2.Text = "";
                    lblCount1.Content = "0";
                    lblCount2.Content = "0";
                    return;
                }

                if (!int.TryParse(strLongStr, out intLongStr))
                {
                    txtResult1.Text = "";
                    txtResult2.Text = "";
                    lblCount1.Content = "0";
                    lblCount2.Content = "0";
                    return;
                }

                if (intStartStr < 1)
                {
                    MessageBox.Show("開始桁は不正です。",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }

                if (intLongStr < 0)
                {
                    MessageBox.Show("長さは不正です。",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }

                if (!txtFixData1.Text.Equals(""))
                {
                    txtResult1.Text = SubstringByte(txtFixData1.Text, intStartStr - 1, intLongStr);
                    lblCount1.Content = sjisEnc.GetByteCount(txtFixData1.Text);
                }

                if (!txtFixData2.Text.Equals(""))
                {
                    txtResult2.Text = SubstringByte(txtFixData2.Text, intStartStr - 1, intLongStr);
                    lblCount2.Content = sjisEnc.GetByteCount(txtFixData2.Text);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// SubstringByte
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string SubstringByte(string value, int startIndex, int length)
        {
            try
            {
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                byte[] byteArray = sjisEnc.GetBytes(value);



                if (byteArray.Length < startIndex + 1)
                    return "";

                if (byteArray.Length < startIndex + length)
                    length = byteArray.Length - startIndex;

                string cut = sjisEnc.GetString(byteArray, startIndex, length);

                // 最初の文字が全角の途中で切れていた場合はカット
                string left = sjisEnc.GetString(byteArray, 0, startIndex + 1);
                char first = value[left.Length - 1];
                if (0 < cut.Length && first != cut[0])
                    cut = cut.Substring(1);

                // 最後の文字が全角の途中で切れていた場合はカット
                left = sjisEnc.GetString(byteArray, 0, startIndex + length);

                char last = value[left.Length - 1];
                if (0 < cut.Length && last != cut[cut.Length - 1])
                    cut = cut.Substring(0, cut.Length - 1);

                return cut.Replace(" ", ".");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return "";
            }
        }

        /// <summary>
        /// txtSubstring_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSubstring_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                SubGetString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
