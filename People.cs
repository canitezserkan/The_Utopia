using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Utopia
{
    public partial class People : Form
    {
        public People()
        {
            InitializeComponent();
        }
        public List<v_Kisiler> kisiler { get; set; }
        
        private void People_Load(object sender, EventArgs e)
        {   
            clear();
        }


        private void txt_TextChanged(object sender, EventArgs e)
        {
            Type type = sender.GetType();
            if (type.ToString() == "System.Windows.Forms.MaskedTextBox")
            {
                MaskedTextBox filter = (MaskedTextBox)sender;
                VeriGetir();
            }
            else
            {
                DateTimePicker date = (DateTimePicker)sender;
                date.Name = "used";
                VeriGetir();
            }
            dtgridview.DataSource = kisiler;
            UtopiaEntities db = new UtopiaEntities();
            kisiler = db.v_Kisiler.ToList();
            RowNumber(dtgridview);
        }

        public void VeriGetir()
        {
            kisiler = kisiler.Where(x => x.Name.StartsWith(txt_Name.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.Surname.StartsWith(txt_Surname.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.Age.ToString().StartsWith(txt_Age.Text)).ToList();
            kisiler = kisiler.Where(x => x.Alive.ToString().StartsWith(txt_Alive.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.Height.StartsWith(txt_Height.Text)).ToList();
            kisiler = kisiler.Where(x => x.Weight.StartsWith(txt_Weight.Text)).ToList();
            kisiler = kisiler.Where(x => x.EyeColor.StartsWith(txt_Eyecolor.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.HairColor.StartsWith(txt_Haircolor.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.CityName.StartsWith(txt_CityName.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.StatusName.StartsWith(txt_MarriageStatu.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.OccupationName.StartsWith(txt_Occupation.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            kisiler = kisiler.Where(x => x.Salary.ToString().StartsWith(txt_Salary.Text)).ToList();
            kisiler = kisiler.Where(x => x.GenderName.StartsWith(txt_Gender.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            if (dPicker.Name == "used")
            {
                kisiler = kisiler.Where(x => x.Birthday.ToString() == dPicker.Text).ToList();
            }

        }

        private void btn_clearFilter_Click(object sender, EventArgs e)
        {
            clear();
        }


        public void clear()
        {
            this.Controls.OfType<MaskedTextBox>()
             .Where(textBox => textBox.Tag.ToString() == "txt").ToList()
             .ForEach(textBox => textBox.Clear());
            UtopiaEntities db = new UtopiaEntities();
            kisiler = db.v_Kisiler.ToList();
            dPicker.MaxDate = System.DateTime.Today; //Girilebilecek en yüksek tarih
            dPicker.Value = System.DateTime.Today; //Form Load olduğunda gözükecek default tarih
            dtgridview.DataSource = kisiler; //Bunu bir üst satıra yazınca Load olunca sayfa, veriler gelmiyor. Çünkü, datePicker'ın TextChanged Event'i tetikleniyor ve sorgu sonucunda yeni tarihte doğmuş olan kimse bulunamıyor.          
            dPicker.Name = "unused";

            RowNumber(dtgridview);

        }

        public void RowNumber(DataGridView datagrd)
        {
            for (int i = 0; i < datagrd.Rows.Count; i++)
            {
                datagrd.Rows[i].Cells[0].Value = i + 1;

            }
        }

        private void dtgridview_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {           
            if (e.RowIndex>=0)
            {
                int personId = Convert.ToInt32(dtgridview.Rows[e.RowIndex].Cells[1].Value);
                PersonDetails personDetails = new PersonDetails();
                personDetails.PersonID = personId;
                personDetails.PreviousPersonID.Add(personId);

                //personDetails.Show();
                personDetails.Focus();
                personDetails.Visible = false; //Show dialog çalışması için formun false olması lazım
                personDetails.ShowDialog();
            }
        }
    }
}
