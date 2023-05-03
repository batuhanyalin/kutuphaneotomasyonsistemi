﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KutuphaneOtomasyonSistemi
{
    public partial class yayinevipaneli : Form
    {
        public yayinevipaneli()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(BaglanClass.sqlconnection);

        void verisayisi()
        {
            lblverisayisi.Text = (dataGridView1.RowCount - 1).ToString();
        }

        //METOTLAR------------------------------------------------------------------------
        public void gunlukzaman()
        {
            DateTime gunlukzaman = DateTime.Now;
            lblgunlukzaman.Text = gunlukzaman.ToString("g");
        }
        void kullaniciadi()
        {
            string deger;
            SqlCommand cmd = new SqlCommand("select * from tblgirislog", baglanti);
            baglanti.Open();
            deger = (string)cmd.ExecuteScalar();
            baglanti.Close();
            txtpersonelkullaniciadi.Text = deger;
        }

        void listele()
        {
            SqlCommand komutlistele = new SqlCommand("execute yayinevipaneli", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komutlistele);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            verisayisi();
            temizle();
        }

        void guncelle()
        {

            DialogResult secenek = MessageBox.Show("Yayınevi bilgileri güncellensin mi?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secenek == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komutguncelle = new SqlCommand("update tblyayinevi set YayineviAd=@p2 where YayineviID=@p1", baglanti);
                komutguncelle.Parameters.AddWithValue("@p1", txtid.Text);
                komutguncelle.Parameters.AddWithValue("@p2", txtad.Text);
                komutguncelle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Yayınevi bilgileri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
            }
            else { }
        }

        void kaydet()
        {
            DialogResult secenek = MessageBox.Show("Yeni yayınevi kaydedilsin mi?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secenek == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komutkaydet = new SqlCommand("insert into tblyayinevi (YayineviAd) values (@p1)", baglanti);
                komutkaydet.Parameters.AddWithValue("@p1", txtad.Text);
                komutkaydet.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Yeni yayınevi başarıyla başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
            }
            else { }
        }
         void temizle()
        {
            txtid.Text = "";
            txtad.Text = "";
            txtadarama.Text = "";
        }
        void sil()
        {
            //messageboxdan gelen cevaba göre işlem yaptırma.
            DialogResult secenek = MessageBox.Show("Seçilen yayınevi silinsin mi?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secenek == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komutsil = new SqlCommand("delete from tblyayinevi where YayineviID=@p1 ", baglanti);
                komutsil.Parameters.AddWithValue("@p1", txtid.Text);
                komutsil.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Yayınevi başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
            }
            else { }
        }

        //MENU STRIP BAR ------------------------------------------------------
        private void anasayfaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yonetimpaneli yp = new yonetimpaneli();
            yp.Show();
            this.Hide();
        }

        private void programHakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hakkindapaneli hp = new hakkindapaneli();
            hp.Show();
        }

        private void ÇIKIŞToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cikis x = new cikis();
            x.cikisbaglantisi();
            girispaneli gp = new girispaneli();
            gp.Show();
            this.Hide();
        }
        //BUTONLAR ---------------------------------------------------------
        private void btnsifirla_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void btnlistele_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void btnguncelle_Click(object sender, EventArgs e)
        {
            guncelle();
        }

        private void btnkaydet_Click(object sender, EventArgs e)
        {
            //Boş bilgi girmeyi önleyen şart yazdım.

            if ((string.IsNullOrEmpty(txtad.Text)))
            {
                MessageBox.Show("Tüm Bilgileri Eksiksiz ve Doğru Girmeden Kayıt Yapamazsınız.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                kaydet();
            }
        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            sil();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtid.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtad.Text= dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void yayinevipaneli_Load(object sender, EventArgs e)
        {
            kullaniciadi();
            listele();
            gunlukzaman();
        }

        private void txtadarama_TextChanged(object sender, EventArgs e)
        {
            SqlCommand komutara = new SqlCommand("select YayineviID as 'ID', YayineviAd as 'YayınEvi' from tblyayinevi where YayineviAd like '%" + txtadarama.Text + "%'", baglanti);
            komutara.Parameters.AddWithValue("@p1", txtadarama.Text);
            SqlDataAdapter da = new SqlDataAdapter(komutara);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            verisayisi();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel=true; 
        }
    }
}
