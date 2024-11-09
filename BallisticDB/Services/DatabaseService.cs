using BallisticDB.Messages;
using BallisticDB.Settings;
using BallisticDB.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BallisticDB.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private SqliteConnection? _con;
        private long _maxRifleId = 0;
        private long _maxCartridgeId = 0;
        private List<RifleViewModel>? _rifles;
        private List<CartridgeViewModel>? _cartridges;
        private bool _unsavedData = false;

        public DatabaseService(IOptions<AppSettings> appSettings)
        {
            _connectionString = appSettings.Value.SQLitePrefix + appSettings.Value.DbLocation
                + "\\" + appSettings.Value.DbName
                + ";";
            _cartridges = new List<CartridgeViewModel>();
            WeakReferenceMessenger.Default.Register<DataChangedMessage>(this, (r, m) =>
            {
                _unsavedData = true;
            });
        }

        public bool OpenDatabase(bool create = true, bool overWrite = false)
        {
            var result = false;
            if (_connectionString != null)
            {
                _con = new SqliteConnection(_connectionString);
                _con.Open();
            }
            return result;
        }

        public bool CloseDatabase()
        {
            if (_con != null)
            {
                _con.Close();
            }
            return true;
        }

        public List<RifleViewModel>? LoadRifleRecords()
        {
            var sql = "SELECT id, desc, sh, tr, zd, ec, wc from Rifle ORDER BY id";
            _rifles = new List<RifleViewModel>();
            try
            {
                using (var cmd = new SqliteCommand(sql, _con))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                var rifle = new RifleViewModel();
                                rifle.RifleId = Convert.ToInt32(rdr["id"]);
                                rifle.RifleName = Convert.ToString(rdr["desc"]);
                                rifle.ScopeHeight = Convert.ToDouble(rdr["sh"]);
                                rifle.TwistRate = Convert.ToDouble(rdr["tr"]);
                                rifle.ZeroDistance = Convert.ToDouble(rdr["zd"]);
                                rifle.ElevationClicksPerMOA = Convert.ToDouble(rdr["ec"]);
                                rifle.WindageClicksPerMOA = Convert.ToDouble(rdr["wc"]);
                                rifle.RowState = RowStatus.UNCHANGED;
                                _rifles.Add(rifle);
                            }
                        }
                    }
                }
                _maxRifleId = (_rifles.Count == 0 ? 0 : _rifles.Max(p => p.RifleId));
                LoadCartridgeRecords();
                _unsavedData = false;
            }
            catch (Exception ex)
            {

            }
            return _rifles;
        }

        public List<RifleData>? LoadRifleData()
        {
            var sql = "SELECT id, desc, sh, tr, zd, ec, wc from Rifle ORDER BY id";
            var rflData = new List<RifleData>();
            try
            {
                using (var cmd = new SqliteCommand(sql, _con))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                var rifle = new RifleData();
                                rifle.id = Convert.ToInt32(rdr["id"]);
                                rifle.desc = Convert.ToString(rdr["desc"]);
                                rifle.sh = Convert.ToDouble(rdr["sh"]);
                                rifle.tr = Convert.ToDouble(rdr["tr"]);
                                rifle.zd = Convert.ToDouble(rdr["zd"]);
                                rifle.ec = Convert.ToDouble(rdr["ec"]);
                                rifle.wc = Convert.ToDouble(rdr["wc"]);
                                rflData.Add(rifle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rflData;
        }

        public List<CartridgeData> LoadCartridgeData()
        {
            var sql = "SELECT id, rifleid, desc, wt, mv, bc, bl, clbr from Cartridge ORDER BY rifleid, id";
            var crtdgs = new List<CartridgeData>();
            try
            {
                using (var cmd = new SqliteCommand(sql, _con))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                var cartridge = new CartridgeData();
                                cartridge.id = Convert.ToInt64(rdr["id"]);
                                cartridge.rifleid = Convert.ToInt64(rdr["rifleid"]);
                                cartridge.desc = Convert.ToString(rdr["desc"]);
                                cartridge.wt = Convert.ToInt32(rdr["wt"]);
                                cartridge.mv = Convert.ToInt32(rdr["mv"]);
                                cartridge.bc = Convert.ToDouble(rdr["bc"]);
                                cartridge.bl = Convert.ToDouble(rdr["bl"]);
                                cartridge.clbr = Convert.ToDouble(rdr["clbr"]);
                                crtdgs.Add(cartridge);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return crtdgs;
        }

        public RifleViewModel AddRifle(RifleViewModel rifle)
        {
            bool retval = false;
            long result = 0;
            rifle.RifleId = ++_maxRifleId;
            rifle.RowState = RowStatus.INSERTED;
            _rifles.Add(rifle);
            return rifle;
        }

        public CartridgeViewModel AddCartridge(CartridgeViewModel cartridge)
        {
            cartridge.CartridgeId = ++_maxCartridgeId;
            cartridge.RowState = RowStatus.INSERTED;
            _cartridges.Add(cartridge);
            return cartridge;
        }

        public bool DeleteRifle(RifleViewModel rifle)
        {
            var result = true;
            if (DeleteAllCartridges(rifle.RifleId))
            {
                rifle.RowState = RowStatus.DELETED;
                return result;
            }
            return false;
        }

        private bool DeleteAllCartridges(long rifleId)
        {
            var result = true;
            var x = (from c in _cartridges where c.RifleId == rifleId select c).ToList<CartridgeViewModel>();
            foreach (var c in x)
            {
                DeleteCartridge(c);
            }
            return result;
        }

        public bool DeleteCartridge(CartridgeViewModel cartridge)
        {
            var result = true;
            cartridge.RowState = RowStatus.DELETED;
            return result;
        }

        public List<CartridgeViewModel> LoadCartridgeRecords()
        {
            var sql = "SELECT id, rifleid, desc, wt, mv, bc, bl, clbr from Cartridge ORDER BY rifleid, id";
            _cartridges = new List<CartridgeViewModel>();
            try
            {
                using (var cmd = new SqliteCommand(sql, _con))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                var cartridge = new CartridgeViewModel();
                                cartridge.CartridgeId = Convert.ToInt64(rdr["id"]);
                                cartridge.RifleId = Convert.ToInt64(rdr["rifleid"]);
                                cartridge.CartridgeName = Convert.ToString(rdr["desc"]);
                                cartridge.Weight = Convert.ToDouble(rdr["wt"]);
                                cartridge.MuzzleVelocity = Convert.ToDouble(rdr["mv"]);
                                cartridge.BallisticCoefficient = Convert.ToDouble(rdr["bc"]);
                                cartridge.BulletLength = Convert.ToDouble(rdr["bl"]);
                                cartridge.Calibre = Convert.ToDouble(rdr["clbr"]);
                                cartridge.RowState = RowStatus.UNCHANGED;
                                _cartridges.Add(cartridge);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return _cartridges;
        }

        public List<CartridgeViewModel> GetCartridgesByRifleId(RifleViewModel rifle)
        {
            long rID = 0;
            if (rifle != null)
            {
                rID = rifle.RifleId;
            }
            var x = (from c in _cartridges where c.RifleId == rID && c.RowState != RowStatus.DELETED select c).ToList<CartridgeViewModel>();
            _maxCartridgeId = (x.Count == 0 ? 0 : x.Max(c => c.CartridgeId));
            return x;
        }

        public void Save()
        {
            InsertRifles();
            InsertCartridges();
            UpdateCartridges();
            UpdateRifles();
            DeleteCartridges();
            DeleteRifles();
            _unsavedData = false;
        }

        private bool InsertRifles()
        {
            var result = 0;
            var sql = "INSERT INTO Rifle (id, desc, sh, tr, zd, ec, wc ) VALUES (@id, @desc, @sh, @tr, @zd, @ec, @wc)";

            try
            {
                var rfls = (from r in _rifles where r.RowState == RowStatus.INSERTED select r).ToList<RifleViewModel>();
                foreach (var row in rfls)
                {
                    using (var cmd = new SqliteCommand(sql, _con))
                    {
                        cmd.Parameters.AddWithValue("@id", row.RifleId);
                        cmd.Parameters.AddWithValue("@desc", row.RifleName);
                        cmd.Parameters.AddWithValue("@sh", row.ScopeHeight);
                        cmd.Parameters.AddWithValue("@tr", row.TwistRate);
                        cmd.Parameters.AddWithValue("@zd", row.ZeroDistance);
                        cmd.Parameters.AddWithValue("@ec", row.ElevationClicksPerMOA);
                        cmd.Parameters.AddWithValue("@wc", row.WindageClicksPerMOA);
                        result = cmd.ExecuteNonQuery();
                        if ((result > 0) == false)
                        {
                            throw new Exception("Rifle insert failed");
                        }
                        row.RowState = RowStatus.UNCHANGED;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool InsertCartridges()
        {
            var result = 0;
            var sql = "INSERT INTO cartridge (id, rifleid, desc, wt, mv, bc, bl, clbr) VALUES (@id, @rifleid, @desc, @wt, @mv, @bc, @bl, @clbr)";

            try
            {
                var ctgs = (from c in _cartridges where c.RowState == RowStatus.INSERTED select c).ToList<CartridgeViewModel>();
                foreach (var row in ctgs)
                {
                    using (var cmd = new SqliteCommand(sql, _con))
                    {
                        cmd.Parameters.AddWithValue("@id", row.CartridgeId);
                        cmd.Parameters.AddWithValue("@rifleid", row.RifleId);
                        cmd.Parameters.AddWithValue("@desc", row.CartridgeName);
                        cmd.Parameters.AddWithValue("@wt", row.Weight);
                        cmd.Parameters.AddWithValue("@mv", row.MuzzleVelocity);
                        cmd.Parameters.AddWithValue("@bc", row.BallisticCoefficient);
                        cmd.Parameters.AddWithValue("@bl", row.BulletLength);
                        cmd.Parameters.AddWithValue("@clbr", row.Calibre);
                        result = cmd.ExecuteNonQuery();
                        if ((result > 0) == false)
                        {
                            throw new Exception("Cartridge insert failed");
                        }
                        row.RowState = RowStatus.UNCHANGED;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool UpdateRifles()
        {
            var result = 0;
            var sql = "UPDATE Rifle SET desc = @desc, sh = @sh, tr = @tr, zd = @zd, ec = @ec, wc = @wc WHERE id = @id";

            try
            {
                var rfls = (from r in _rifles where r.RowState == RowStatus.UPDATED select r).ToList<RifleViewModel>();
                foreach (var row in rfls)
                {
                    using (var cmd = new SqliteCommand(sql, _con))
                    {
                        cmd.Parameters.AddWithValue("@id", row.RifleId);
                        cmd.Parameters.AddWithValue("@desc", row.RifleName);
                        cmd.Parameters.AddWithValue("@sh", row.ScopeHeight);
                        cmd.Parameters.AddWithValue("@tr", row.TwistRate);
                        cmd.Parameters.AddWithValue("@zd", row.ZeroDistance);
                        cmd.Parameters.AddWithValue("@ec", row.ElevationClicksPerMOA);
                        cmd.Parameters.AddWithValue("@wc", row.WindageClicksPerMOA);
                        result = cmd.ExecuteNonQuery();
                        if ((result > 0) == false)
                        {
                            throw new Exception("Rifle update failed");
                        }
                        row.RowState = RowStatus.UNCHANGED;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool UpdateCartridges()
        {
            var result = 0;
            var sql = "UPDATE Cartridge SET desc = @desc, wt = @wt, mv = @mv, bc = @bc, bl = @bl, clbr = @clbr WHERE id = @id AND rifleid = @rifleid";

            try
            {
                var ctgs = (from c in _cartridges where c.RowState == RowStatus.UPDATED select c).ToList<CartridgeViewModel>();
                foreach (var row in ctgs)
                {
                    using (var cmd = new SqliteCommand(sql, _con))
                    {
                        cmd.Parameters.AddWithValue("@id", row.CartridgeId);
                        cmd.Parameters.AddWithValue("@rifleid", row.RifleId);
                        cmd.Parameters.AddWithValue("@desc", row.CartridgeName);
                        cmd.Parameters.AddWithValue("@wt", row.Weight);
                        cmd.Parameters.AddWithValue("@mv", row.MuzzleVelocity);
                        cmd.Parameters.AddWithValue("@bc", row.BallisticCoefficient);
                        cmd.Parameters.AddWithValue("@bl", row.BulletLength);
                        cmd.Parameters.AddWithValue("@clbr", row.Calibre);
                        result = cmd.ExecuteNonQuery();
                        if ((result > 0) == false)
                        {
                            throw new Exception("Cartridge update failed");
                        }
                        row.RowState = RowStatus.UNCHANGED;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool DeleteCartridges()
        {
            var result = 0;
            var sql = "DELETE FROM Cartridge WHERE id = @id AND rifleid = @rifleid";

            try
            {
                var ctgs = (from c in _cartridges where c.RowState == RowStatus.DELETED select c).ToList<CartridgeViewModel>();
                foreach (var row in ctgs)
                {
                    using (var cmd = new SqliteCommand(sql, _con))
                    {
                        cmd.Parameters.AddWithValue("@id", row.CartridgeId);
                        cmd.Parameters.AddWithValue("@rifleid", row.RifleId);
                        result = cmd.ExecuteNonQuery();
                        if ((result > 0) == false)
                        {
                            throw new Exception("Cartridge delete failed");
                        }
                        _cartridges.Remove(row);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool DeleteRifles()
        {
            var result = 0;
            var sql = "DELETE FROM Rifle WHERE id = @id";

            try
            {
                var rfls = (from r in _rifles where r.RowState == RowStatus.DELETED select r).ToList<RifleViewModel>();
                foreach (var row in rfls)
                {
                    using (var cmd = new SqliteCommand(sql, _con))
                    {

                        cmd.Parameters.AddWithValue("@id", row.RifleId);
                        result = cmd.ExecuteNonQuery();
                        if ((result > 0) == false)
                        {
                            throw new Exception("Cartridge delete failed");
                        }
                        _rifles.Remove(row);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
