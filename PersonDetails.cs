using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Utopia
{
    public partial class PersonDetails : Form
    {
        public int PersonID
        {
            get; set;
        }

        public List<int> PreviousPersonID = new List<int>();
        public List<string> PersonName = new List<string>();

        public List<v_Kisiler> kisiler { get; set; } //Kişiler view tablosunu database'ten aldık
        public List<PersonRelationship1_Result> P_PersonRelation { get; set; } //PersonRelationship1_Result, procedure'ün getireceği sonuç tablosu ya da değeridir.
        public PersonDetails()
        {
            InitializeComponent();
        }

        private void PersonDetails_Load(object sender, EventArgs e)
        {
            PersonRelationList(PersonID, true);

        }

        private void PersonRelationList(int Id, bool test)
        {
            UtopiaEntities db = new UtopiaEntities();
            kisiler = db.v_Kisiler.Where(x => x.Id == PersonID).ToList();
            if (test)
            {
                if (PersonName.Count ==0)
                    PersonName.Add(kisiler[0].Name + " " + kisiler[0].Surname); 
                else
                    PersonName.Add("> " + kisiler[0].Name + " " + kisiler[0].Surname);
            }

            if (PersonName.Count <= 5)
            {
                pnl_names.Controls.Clear();
                for (int i = 0; i < PersonName.Count; i++)
                {
                    Label lbl_prs = new Label();
                    lbl_prs.Width = 500;
                    lbl_prs.Text = PersonName[i];
                    lbl_prs.Dock = DockStyle.Fill;
                    pnl_names.Controls.Add(lbl_prs, i, 0);
                }
            }
            else
            {
                pnl_names.Controls.Clear();
                int a = 1;
                int b = 4;
                for (int i = 0; i < 5; i++)
                {
                    Label lbl_prs = new Label();
                    lbl_prs.Width = 500;
                    lbl_prs.Text = PersonName[PersonName.Count - a++];
                    lbl_prs.Dock = DockStyle.Fill;
                    pnl_names.Controls.Add(lbl_prs, b--, 0);
                } 
            }

            P_PersonRelation = db.PersonRelationship1(Id).ToList(); //PersonID'yi prosedüre parametre olarak verdik.
            dtGridView_Relationship.DataSource = P_PersonRelation;

            RowNumber(dtGridView_Relationship);
            
        }

        public void RowNumber(DataGridView datagrd)
        {
            for (int i = 0; i < datagrd.Rows.Count; i++)
            {
                datagrd.Rows[i].Cells[0].Value = i + 1; //DataGridView'ın En solundaki No'nun Cell index'i 0'dır. Görünmez olan Id'Nin ise 1.
            }
        }

        private void dtGridView_Relationship_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                PersonID = Convert.ToInt32(dtGridView_Relationship.Rows[e.RowIndex].Cells[1].Value); //GridView'da gözükmeyen gizli ID kolonunun Hücre index'i 1.
                PersonRelationList(PersonID,true);
                PreviousPersonID.Add(PersonID);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PreviousPersonID.Count>1) //Küçük form açıldıktan hemen sonra geriye basarsak butonun çalışmamasını sağlamamız lazım yoksa hata alırız.
            {
                var id = PreviousPersonID[PreviousPersonID.Count - 2];
                PersonName.RemoveAt(PersonName.Count - 1);
                PersonRelationList(id,false);

                PreviousPersonID.RemoveAt(PreviousPersonID.Count - 1); 
            }
        }
    }
}
