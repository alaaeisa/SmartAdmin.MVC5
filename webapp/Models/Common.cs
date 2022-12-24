﻿using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class Common

    {
        public  bool balancIn(int  ItemId,int StoreID , decimal  Quantity , CarWorkShopEntities db)
        {
           
            try
            {
                StoresBalance _store = new StoresBalance();
                _store = db.StoresBalances.Where(x => x.ItemID == ItemId && x.StoreID == StoreID).FirstOrDefault();
                if (_store == null)
                {
                    StoresBalance st = new StoresBalance();
                    st.Quantity = Quantity;
                    st.ItemID = ItemId;
                    st.StoreID = StoreID;   
                    db.StoresBalances.Add(st);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    decimal? old = _store.Quantity;
                    decimal? balanceQty = old + Quantity;

                    if (balanceQty < 0)
                    {
                        return false;
                    }
                    else
                    {
                        _store.Quantity = balanceQty??0;
                      
                        db.Entry(_store).State = System.Data.Entity.EntityState.Modified;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogs.logData("Common", "balancIn", ex.Message, "");
                return false;
            }
          
          
        }

        public bool balancOut(int ItemId,int StoreID, decimal Quantity, CarWorkShopEntities db)
        {
            try
            {
                StoresBalance _store = new StoresBalance();
                _store = db.StoresBalances.Where(x => x.ItemID == ItemId  && x.StoreID == StoreID).FirstOrDefault();
                if (_store==null)
                {

                    return false;
                }
                else
                {
                    decimal? old = _store.Quantity;
                    decimal ? newBalanceQty = old - Quantity;
                    if (newBalanceQty < 0)
                    {
                        return false;
                    }
                    else
                    {
                        _store.Quantity = newBalanceQty ?? 0;
                        db.Entry(_store).State = System.Data.Entity.EntityState.Modified;
                        return true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                DbLogs.logData("Common", "balancout", ex.Message, "");
                return false;
            }


        }
    }
}