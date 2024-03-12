using System;
using System.Drawing;
using System.Windows.Forms;

namespace Uno.Classes
{
    //This is here to prevent the class from turning into a "design mode" class.
    public class CustomControls { }

    public class CustomLabel : Label
    {
        public Card AssignedCard { get; set; }
        public CustomLabel() : base()
        {
            DoubleBuffered = true;
        }
    }

    public class CustomButton : Button
    {
        public CustomButton() : base()
        {
            DoubleBuffered = true;
        }
    }

    public class CustomTextBox : TextBox
    {
        public CustomTextBox() : base()
        {
            DoubleBuffered = true;
        }
    }

    public class CustomRichTextBox : RichTextBox
    {
        public CustomRichTextBox() : base()
        {
            DoubleBuffered = true;
        }
    }

    public class ColorSelectionPanel : Panel
    {
        public ColorSelectionPanel(frmUno form1) : base()
        {
            if (form1.InvokeRequired)
            {
                form1.Invoke(new Action(ContructPanel));
            }
            else
            {
                ContructPanel();
            }

            void ContructPanel()
            {
                Parent = form1;
                BackColor = Color.Gray;

                CardFunctionality cardFunctionality = form1.cardFunctionality;

                this.Size = new System.Drawing.Size(400, 200);

                this.Location = new Point((form1.Size.Width / 2) - (this.Width / 2), (form1.Size.Height / 2) - (this.Height / 2));

                CustomTableLayoutPanel pnlColorSelection = new CustomTableLayoutPanel()
                {
                    Parent = this,
                    Dock = DockStyle.Fill,
                    ColumnCount = 8,
                    RowCount = 3,
                    ColumnStyles =
                {
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 5F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 18.75F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 5F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 18.75F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 5F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 18.75F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 5F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 18.75F},
                new ColumnStyle() {SizeType = SizeType.Percent, Width = 5F},
                },
                    RowStyles =
                {
                    new RowStyle() {SizeType = SizeType.Percent, Height = 15F},
                    new RowStyle() {SizeType = SizeType.Percent, Height = 70F},
                    new RowStyle() {SizeType = SizeType.Percent, Height = 15F},
                }
                };

                CustomLabel lblRed = new CustomLabel()
                {
                    BackColor = System.Drawing.Color.Red,
                    Dock = DockStyle.Fill,
                    Parent = pnlColorSelection,
                    Text = string.Empty
                };
                CustomLabel lblGreen = new CustomLabel()
                {
                    BackColor = System.Drawing.Color.Green,
                    Dock = DockStyle.Fill,
                    Parent = pnlColorSelection,
                    Text = string.Empty
                };
                CustomLabel lblYellow = new CustomLabel()
                {
                    BackColor = System.Drawing.Color.Yellow,
                    Dock = DockStyle.Fill,
                    Parent = pnlColorSelection,
                    Text = string.Empty
                };
                CustomLabel lblBlue = new CustomLabel()
                {
                    BackColor = System.Drawing.Color.Blue,
                    Dock = DockStyle.Fill,
                    Parent = pnlColorSelection,
                    Text = string.Empty
                };

                pnlColorSelection.SetRow(lblRed, 1);
                pnlColorSelection.SetRow(lblGreen, 1);
                pnlColorSelection.SetRow(lblYellow, 1);
                pnlColorSelection.SetRow(lblBlue, 1);

                pnlColorSelection.SetColumn(lblRed, 1);
                pnlColorSelection.SetColumn(lblGreen, 3);
                pnlColorSelection.SetColumn(lblYellow, 5);
                pnlColorSelection.SetColumn(lblBlue, 7);

                lblRed.Click += (sender, e) => cardFunctionality.ChangeGameColor(Card.ColorEnum.Red);
                lblRed.Click += cardFunctionality.CloseColorSelector;
                lblGreen.Click += (sender, e) => cardFunctionality.ChangeGameColor(Card.ColorEnum.Green);
                lblGreen.Click += cardFunctionality.CloseColorSelector;
                lblYellow.Click += (sender, e) => cardFunctionality.ChangeGameColor(Card.ColorEnum.Yellow);
                lblYellow.Click += cardFunctionality.CloseColorSelector;
                lblBlue.Click += (sender, e) => cardFunctionality.ChangeGameColor(Card.ColorEnum.Blue);
                lblBlue.Click += cardFunctionality.CloseColorSelector;
            }
        }
    }

    public class CustomTableLayoutPanel : TableLayoutPanel
    {
        public CustomTableLayoutPanel() : base()
        {
            DoubleBuffered = true;
        }
    }

    public class CustomPictureBox : PictureBox
    {
        public CustomPictureBox() : base()
        {
            DoubleBuffered = true;
        }
    }
}
