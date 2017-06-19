﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using Tipwin.Hash;
using Tipwin.Models;

namespace Tipwin.Repository
{
    public class PlayerDb
    {
        MySqlConnection connection;
        string conString = "SERVER = 'localhost'; "
            + "DATABASE = 'player'; "
            + "UID = 'root'; "
            + "PASSWORD='root'; ";
        public List<Player> GetPlayers()
        {


            connection = new MySqlConnection(conString);
            const string selQuery = "SELECT * FROM player.players;";
            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<Player> listPlayers = new List<Player>();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Player p1 = new Player()
                {
                    Id = Convert.ToInt32(dr["id"]),
                    Oslovljavanje = Convert.ToString(dr["oslovljavanje"]),
                    Ime = Convert.ToString(dr["ime"]),
                    Prezime = Convert.ToString(dr["prezime"]),
                    DatumRodjenja = Convert.ToDateTime(dr["datum_rodjenja"]),
                    Email = Convert.ToString(dr["email"]),
                    EmailPonovo = Convert.ToString(dr["email_ponovo"]),
                    Ulica = Convert.ToString(dr["ulica"]),
                    KucniBroj = Convert.ToString(dr["kucni_broj"]),
                    GradMjesto = Convert.ToString(dr["grad_mjesto"]),
                    PostanskiBroj = Convert.ToInt32(dr["postanski_broj"]),
                    Drzava = Convert.ToString(dr["drzava"]),
                    JezikZaKontakt = Convert.ToString(dr["jezik_za_kontakt"]),
                    BrojTelefona = Convert.ToInt32(dr["broj_telefona"] as int? ?? null),
                    BrojMobitela = Convert.ToInt32(dr["broj_mobitela"] as int? ?? null),
                    KorisnickoIme = Convert.ToString(dr["korisnicko_ime"]),
                    Lozinka = Convert.ToString(dr["lozinka"]),
                    LozinkaPonovo = Convert.ToString(dr["lozinka_ponovo"])
                };
                listPlayers.Add(p1);
            }
            return listPlayers;
        }
        public bool InsertPlayer(Player player)
        {

            string saltHashReturned = HashedPassword.Sha256(player.Lozinka);

            connection = new MySqlConnection(conString);
            const string insQuery = "INSERT INTO players VALUES (@id,@oslovljavanje,@ime,@prezime,@datum_rodjenja,@email,@email_ponovo,@ulica,@kucni_broj,@grad_mjesto,@postanski_broj,@drzava,@jezik_za_kontakt,@broj_telefona,@broj_mobitela,@korisnicko_ime,@lozinka,@lozinka_ponovo)";
            MySqlCommand cmd = new MySqlCommand(insQuery, connection);
            cmd.Parameters.AddWithValue("@id", player.Id);
            cmd.Parameters.AddWithValue("@oslovljavanje", player.Oslovljavanje);
            cmd.Parameters.AddWithValue("@ime", player.Ime);
            cmd.Parameters.AddWithValue("@prezime", player.Prezime);
            cmd.Parameters.AddWithValue("@datum_rodjenja", player.DatumRodjenja);
            cmd.Parameters.AddWithValue("@email", player.Email);
            cmd.Parameters.AddWithValue("@email_ponovo", player.EmailPonovo);
            cmd.Parameters.AddWithValue("@ulica", player.Ulica);
            cmd.Parameters.AddWithValue("@kucni_broj", player.KucniBroj);
            cmd.Parameters.AddWithValue("@grad_mjesto", player.GradMjesto);
            cmd.Parameters.AddWithValue("@postanski_broj", player.PostanskiBroj);
            cmd.Parameters.AddWithValue("@drzava", player.Drzava);
            cmd.Parameters.AddWithValue("@jezik_za_kontakt", player.JezikZaKontakt);
            cmd.Parameters.AddWithValue("@broj_telefona", player.BrojTelefona);
            cmd.Parameters.AddWithValue("@broj_mobitela", player.BrojMobitela);
            cmd.Parameters.AddWithValue("@korisnicko_ime", player.KorisnickoIme);
            cmd.Parameters.AddWithValue("@lozinka", saltHashReturned);
            //  cmd.Parameters.AddWithValue("@lozinka", player.Lozinka);
            cmd.Parameters.AddWithValue("@lozinka_ponovo", saltHashReturned);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

    }
}
