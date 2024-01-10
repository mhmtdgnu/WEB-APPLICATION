using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Entities;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;

namespace WebApplication1.Repositories
{

    public class KategoriRepository : IKategoriRepository
    {
        private readonly ConnectionHelper _connectionHelper;
        public KategoriRepository(ConnectionHelper connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        public void CreateKategori(Kategori kategori)
        {
            var query = "INSERT INTO TBLKATEGORI ( AD, DURUM)" +
                 "VALUES( @AD, @DURUM)";
            var parameters = new DynamicParameters();

            parameters.Add("AD", kategori.AD, DbType.String);
            parameters.Add("DURUM", kategori.DURUM, DbType.Boolean);

            using var connection = _connectionHelper.CreateSqlConnection();
            connection.Execute(query, parameters);
        }

        public void DeleteKategori(int kategoriId)
        {
            var query = "UPDATE TBLKATEGORI SET DURUM = @Durum WHERE ID = @Id";

            using var connection = _connectionHelper.CreateSqlConnection();
            connection.Execute(query, new { ID = kategoriId, DURUM = 0 });
        }

        public IEnumerable<Kategori> GetKategori()
        {
            var query = "SELECT * FROM TBLKATEGORI WHERE DURUM = 1 ";
            using var connection = _connectionHelper.CreateSqlConnection();
            var duyuru = connection.Query<Kategori>(query);
            return duyuru.ToList();

        }

        public Kategori GetKategoriId(int id)
        {
            var query = "SELECT * FROM TBLKATEGORI WHERE ID=@ID";
            using var connection = _connectionHelper.CreateSqlConnection();
            var kategori = connection.QueryFirst<Kategori>(query, new { ID = id });
            return kategori;
        }

        public void UpdateKategori(int kategoriId, Kategori kategori)
        {
            var query = "UPDATE  TBLKATEGORI SET AD=@AD, DURUM=@DURUM" +
             " WHERE ID=@ID";
            var parameters = new DynamicParameters();

            parameters.Add("ID", kategoriId, DbType.Int32);
            parameters.Add("AD", kategori.AD, DbType.String);
            parameters.Add("DURUM", kategori.DURUM, DbType.Boolean);

            using var connection = _connectionHelper.CreateSqlConnection();
            connection.Execute(query, parameters);


        }
    }
}
