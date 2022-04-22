using System.Runtime.InteropServices;
using System.Text;

namespace DrumMachine
{
    public partial class Form1 : Form
    {
        string fileNameT1, fileNameT2, fileNameT3, fileNameT4, nameT1, nameT2, nameT3, nameT4;
        bool[,] checkBoxes = new bool[4,16];
        bool stopPushed = false;
        bool isStoped = true;
        int tempo = 120;
        int tempoMs = 500/4;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);

        public Form1()
        {
            InitializeComponent();
        }


        // Events

        private void secuencer_Checked(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            int parentIndex = checkBox.Parent.TabIndex;
            int childIndex = checkBox.TabIndex;

            if (checkBox.Checked)
                checkBoxes[parentIndex,childIndex] = true;
            else if (!checkBox.Checked)
                checkBoxes[parentIndex,childIndex] = false;
        }

        private async void play_ButtonClickAsync(object sender, EventArgs e)
        {
            if (isStoped)
            {
                isStoped = false;
                stopPushed = false;
                while (!stopPushed)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (checkBoxes[0, j] == true)
                            await PlayMediaFile(nameT1);
                        if (checkBoxes[1, j] == true)
                            await PlayMediaFile(nameT2);
                        if (checkBoxes[2, j] == true)
                            await PlayMediaFile(nameT3);
                        if (checkBoxes[3, j] == true)
                            await PlayMediaFile(nameT4);
                        if (stopPushed == true)
                            break;
                        IlluminateTime(j);
                        await Task.Delay(tempoMs);
                    }
                }
            }
        }

        private void stop_ButtonClick(object sender, MouseEventArgs e)
        {
            stopPushed = true;
            isStoped = true;
            T1Label.ForeColor = T2Label.ForeColor = T3Label.ForeColor = T4Label.ForeColor = T5Label.ForeColor =
                   T6Label.ForeColor = T7Label.ForeColor = T8Label.ForeColor = T9Label.ForeColor = T10Label.ForeColor =
                   T11Label.ForeColor = T12Label.ForeColor = T13Label.ForeColor = T14Label.ForeColor = T15Label.ForeColor =
                   T16Label.ForeColor = Color.Black;
        }

        private void tempo_ValueChanged(object sender, EventArgs e)
        {
            tempo = (int)tempoControl.Value;
            tempoMs = (60000 / tempo)/4;
        }

        private void volume_ValueChanged(object sender, EventArgs e)
        {
            int Volume = ((ushort.MaxValue / 10) * volumeBar.Value);
            uint NewVolume = (((uint)Volume & 0x0000ffff) | ((uint)Volume << 16));
            WaveOutSetVolume(IntPtr.Zero, NewVolume);
        }

        private void open_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "WAV Files (*.wav)|*.wav";

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    var caller = (Control)sender;

                    switch (caller.Name)
                    {
                        case "openT1": 
                            fileNameT1 = fd.FileName;
                            nameT1 = Path.GetFileNameWithoutExtension(fileNameT1);
                            labelT1.Text = nameT1;
                            OpenMediaFile(fileNameT1, nameT1); 
                            break;

                        case "openT2":
                            fileNameT2 = fd.FileName;
                            nameT2 = Path.GetFileNameWithoutExtension(fileNameT2);
                            labelT2.Text = nameT2;
                            OpenMediaFile(fileNameT2, nameT2);
                            break;

                        case "openT3":
                            fileNameT3 = fd.FileName;
                            nameT3 = Path.GetFileNameWithoutExtension(fileNameT3);
                            labelT3.Text = nameT3;
                            OpenMediaFile(fileNameT3, nameT3);
                            break;

                        case "openT4":
                            fileNameT4 = fd.FileName;
                            nameT4 = Path.GetFileNameWithoutExtension(fileNameT4);
                            labelT4.Text = nameT4;
                            OpenMediaFile(fileNameT4, nameT4);
                            break;
                    };                
                }
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            var caller = (Control)sender;

            switch (caller.Name)
            {
                case "deleteT1":
                    fileNameT1 = "";
                    nameT1 = "";
                    labelT1.Text = "...";
                    break;

                case "deleteT2":
                    fileNameT2 = "";
                    nameT2 = "";
                    labelT2.Text = "...";
                    break;

                case "deleteT3":
                    fileNameT3 = "";
                    nameT3 = "";
                    labelT3.Text = "...";
                    break;

                case "deleteT4":
                    fileNameT4 = "";
                    nameT4 = "";
                    labelT4.Text = "...";
                    break;
            }
        }

        private void pad_Click(object sender, MouseEventArgs e)
        {
            var caller = (Control)sender;
            switch (caller.Name)
            {
                case "padT1": 
                    PlayMediaFile(nameT1);
                    break;

                case "padT2":
                    PlayMediaFile(nameT2);
                    break;

                case "padT3":
                    PlayMediaFile(nameT3);
                    break;

                case "padT4":
                    PlayMediaFile(nameT4);
                    break;
            };
        }

        private void pad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                PlayMediaFile(nameT1);
            else if (e.KeyCode == Keys.S)
                PlayMediaFile(nameT2);
            else if (e.KeyCode == Keys.D)
                PlayMediaFile(nameT3);
            else if (e.KeyCode == Keys.F)
                PlayMediaFile(nameT4);
        }


        // Methods

        private void IlluminateTime(int time)
        {
            switch (time)
            {
                case 0:
                    T16Label.ForeColor = Color.Black;
                    T1Label.ForeColor = Color.IndianRed;
                    break;

                case 1:
                    T1Label.ForeColor = Color.Black;
                    T2Label.ForeColor = Color.IndianRed;
                    break;

                case 2:
                    T2Label.ForeColor = Color.Black;
                    T3Label.ForeColor = Color.IndianRed;
                    break;

                case 3:
                    T3Label.ForeColor = Color.Black;
                    T4Label.ForeColor = Color.IndianRed;
                    break;

                case 4:
                    T4Label.ForeColor = Color.Black;
                    T5Label.ForeColor = Color.IndianRed;
                    break;

                case 5:
                    T5Label.ForeColor = Color.Black;
                    T6Label.ForeColor = Color.IndianRed;
                    break;

                case 6:
                    T6Label.ForeColor = Color.Black;
                    T7Label.ForeColor = Color.IndianRed;
                    break;

                case 7:
                    T7Label.ForeColor = Color.Black;
                    T8Label.ForeColor = Color.IndianRed;
                    break;

                case 8:
                    T8Label.ForeColor = Color.Black;
                    T9Label.ForeColor = Color.IndianRed;
                    break;

                case 9:
                    T9Label.ForeColor = Color.Black;
                    T10Label.ForeColor = Color.IndianRed;
                    break;

                case 10:
                    T10Label.ForeColor = Color.Black;
                    T11Label.ForeColor = Color.IndianRed;
                    break;

                case 11:
                    T11Label.ForeColor = Color.Black;
                    T12Label.ForeColor = Color.IndianRed;
                    break;

                case 12:
                    T12Label.ForeColor = Color.Black;
                    T13Label.ForeColor = Color.IndianRed;
                    break;

                case 13:
                    T13Label.ForeColor = Color.Black;
                    T14Label.ForeColor = Color.IndianRed;
                    break;

                case 14:
                    T14Label.ForeColor = Color.Black;
                    T15Label.ForeColor = Color.IndianRed;
                    break;

                case 15:
                    T15Label.ForeColor = Color.Black;
                    T16Label.ForeColor = Color.IndianRed;
                    break;
            }
        }

        private void OpenMediaFile(string fileName, string name)
        {
            string playCommand = "Open \"" + fileName + "\" type waveaudio alias " + name;
            mciSendString(playCommand, null, 0, IntPtr.Zero);
        }

        private Task PlayMediaFile(string name)
        {
            string playCommand = "Seek " + name + " to start";
            string playCommnad2 = "Play " + name;
            mciSendString(playCommand, null, 0, IntPtr.Zero);
            mciSendString(playCommnad2, null, 0, IntPtr.Zero);
            return Task.FromResult(0);
        }












    }
}