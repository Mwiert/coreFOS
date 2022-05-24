﻿using Microsoft.AspNetCore.Mvc;
using CFOS.EntityLayer.Concretes;
using CFOS.ServiceLayer.Concretes.dotnetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace CoreFOS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        Materials materials = new Materials();
        CoatingInfo coatingInfo = new CoatingInfo();
        List<CoatingInfo> coatingInfos = new List<CoatingInfo>();
        Machine machine = new Machine();
        Order order = new Order();
        RawMaterial rawMaterial = new RawMaterial();
        Calisanlar calisanlar = new Calisanlar();
        List<Customer> customerList = new List<Customer>();
        Customer customer = new Customer();
        List<Calisanlar> calisanlars = new List<Calisanlar>();
        List<Materials> MaterialsList = new List<Materials>();
        List<Machine> Machines = new List<Machine>();
        List<Order> Orders = new List<Order>();
        List<RawMaterial> rawMaterials = new List<RawMaterial>();
        List<Product> products = new List<Product>();
        Product product = new Product();
        AdminService AdminService = new AdminService();
        OtherService OtherService = new OtherService();
        CalisanService CalisanService = new CalisanService();
        UserService userService = new UserService();
        public static string Email;
        
        public IActionResult Index(string email)
        {
            Email = email;
            return View("~/Views/Admin/AdminPanel.cshtml");
        }
        public IActionResult Yetkiliislem(int id)
        {
            calisanlars = AdminService.ListCalisan();
            ViewBag.CalisanList = calisanlars;
            customerList = CalisanService.listCustomer();
            ViewBag.CustomerList = customerList;

            if (id != 0)
            {
                AdminService.insert(id);
                calisanlars = AdminService.ListCalisan();
                ViewBag.CalisanList = calisanlars;
            }
            return View("~/Views/Admin/Yetkiliislem.cshtml");
            // return View("~/Views/Admin/Yetkiliislem.cshtml");
        } ///eger üye halihazırda çalışan ise ekleme işlemi yapılmasın. kontrol yap
        public IActionResult DeleteCalisan(int id)
        {
            calisanlars = AdminService.ListCalisan();
            ViewBag.CalisanList = calisanlars;
            customerList = CalisanService.listCustomer();
            ViewBag.CustomerList = customerList;

            if (id != 0)
            {
                AdminService.deleteCalisan(id);
                calisanlars = AdminService.ListCalisan();
                ViewBag.CalisanList = calisanlars;
            }
            return View("~/Views/Admin/Yetkiliislem.cshtml");
        }
        public IActionResult AddCustomer(string Name, string Surname, string TcNo, string PhoneNumber, string Adress, string Email, string Password)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return View("~/Views/Admin/AddCustomer.cshtml");
                }
                else
                {
                    Customer customer = new Customer()
                    {
                        UserAd = Name,
                        UserSoyad = Surname,
                        UserTcNo = TcNo,
                        UserPhoneNumber = PhoneNumber,
                        UserAdress = Adress,
                        UserEmail = Email,
                        UserPassword = Password
                    };
                    userService.userRegister(customer);
                    return View("~/Views/Admin/AddCustomer.cshtml");
                }
                return View("~/Views/Admin/AddCustomer.cshtml");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IActionResult ListProduct()
        {
            products = CalisanService.listProduct();
            ViewBag.ProductList = products;
            return View("~/Views/Admin/ListProduct.cshtml");
        }
        public IActionResult DeleteProduct(int id)
        {
            CalisanService.deleteProduct(id);
            products = CalisanService.listProduct();
            ViewBag.ProductList = products;
            return View("~/Views/Admin/ListProduct.cshtml");
        }
        public IActionResult AddProduct(string UrunAdi,string UrunKodu,double OnCap,double ArkaCap,double HelisBoyu,double HamBoy,int AgizSayisi, int HelisAcisi)
        {
            if (!string.IsNullOrEmpty(UrunAdi))
            {
                product.UrunAdi = UrunAdi;
                product.UrunKodu = UrunKodu;
                product.ArkaCap = ArkaCap;
                product.HelisBoyu = HelisBoyu;
                product.HamBoy = HamBoy;
                product.AgizSayisi = AgizSayisi;
                product.HelisAcisi = HelisAcisi;
                product.OnCap = OnCap;
                CalisanService.insertProdut(product);
                products = CalisanService.listProduct();
                ViewBag.ProductList = products;
                return View("~/Views/Admin/ListProduct.cshtml");
            }

            return View("~/Views/Admin/AddProduct.cshtml");
        }
        public IActionResult AddMaterial(int MaterialLenght, int MaterialCount, string MaterialExp)
        {
            if (MaterialLenght != 0)
            {
                materials.MalzemeUzunluk = MaterialLenght;
                materials.MalzemeAdedi = MaterialCount;
                materials.MalzemeAciklama = MaterialExp;
                OtherService.MaterialAdd(materials);
                MaterialsList = OtherService.MaterialList();
                ViewBag.MaterialList = MaterialsList;
                return View("~/Views/Admin/ListMaterial.cshtml");
            }

            return View("~/Views/Admin/AddMaterial.cshtml");
        }
        public IActionResult DeleteMaterial(int id)
        {
            MaterialsList = OtherService.MaterialList();
            ViewBag.MaterialList = MaterialsList;
            if (id != 0)
            {
                OtherService.DeleteMaterial(id);
                MaterialsList = OtherService.MaterialList();
                ViewBag.MaterialList = MaterialsList;
            }
            return View("~/Views/Admin/ListMaterial.cshtml");
        }
        public IActionResult ListMaterial()
        {
            //malzeme adı neden yok?
            MaterialsList = OtherService.MaterialList();
            ViewBag.MaterialList = MaterialsList;
            return View("~/Views/Admin/ListMaterial.cshtml");
        }
        public IActionResult AddOrder(int HammadeId, int KaplamaId, bool KesmeOK, bool TaslamaOK, bool BilemeOK, bool KumlamaOK, int MachineId, int MusteriId) // düzeltilecek
        {
            if (HammadeId != 0)
            {

                coatingInfo.KaplamaAdi = OtherService.GetCoationgInfoNameById(HammadeId);
                rawMaterial.HammaddeAdi = OtherService.GetRawMaterialNameById(KaplamaId);
                order.Hammadde.HammaddeId = HammadeId;
                order.Kaplama.KaplamaNo = KaplamaId;
                order.TamamlanmaSuresi = 3000;
                order.SiparisTarihi = DateTime.Now;
                order.SevkDurumu = false;
                order.Calisan.CalisanId = 1; //Claimsten Mevcut kullanıcı id çek ve yazdir
                order.TahminiSure = 3000; // makine öğrenmesi algoritmasi tahmin ettirelecekti
                order.SevkTarihi = DateTime.MaxValue;
                order.Musteri.UserId = MusteriId;
                order.Makine.MakineId = MachineId;
                if (KesmeOK != true)
                {
                    order.Kesme = false;
                }
                else
                {
                    order.Kesme = true;
                }
                if (TaslamaOK != true)
                {
                    order.Kesme = false;
                }
                else
                {
                    order.Kesme = true;
                }
                if (BilemeOK != true)
                {
                    order.Bileme = false;
                }
                else
                {
                    order.Bileme = true;
                }
                if (KumlamaOK != true)
                {
                    order.Kumlama = false;
                }
                else
                {
                    order.Kumlama = true;
                }
            }
            else
            {
                Machines = OtherService.MachineList();
                ViewBag.MachineList = Machines;
                customerList = CalisanService.listCustomer();
                ViewBag.CustomerList = customerList;
                coatingInfos = OtherService.CoatingInfoList();
                ViewBag.CoatingInfoList = coatingInfos;
                rawMaterials = OtherService.rawMaterialList();
                ViewBag.RawMaterialList = rawMaterials;
            }

            return View("~/Views/Admin/AddOrder.cshtml");
        }
        public IActionResult DeleteOrder(int id)
        {
            AdminService.getCustomerById(id);
            Orders = CalisanService.listOrder();
            ViewBag.OrderList = Orders;
            if (id != 0)
            {
                CalisanService.deleteOrder(id);
                Orders = CalisanService.listOrder();
                ViewBag.Orders = Orders;
            }
            return View("~/Views/Admin/DeleteOrder.cshtml");
        }
        public IActionResult ListOrder()
        {
            Orders = CalisanService.listOrder();
            ViewBag.OrderList = Orders;
            return View("~/Views/Admin/ListOrder.cshtml");
        }
        public IActionResult AddMachine(string MakineAdi)
        {
            if (!string.IsNullOrEmpty(MakineAdi))
            {
                machine.MakineAdi = MakineAdi;

                OtherService.MachinelAdd(machine);
                Machines = OtherService.MachineList();
                ViewBag.MachineList = Machines;
                return View("~/Views/Admin/ListMachine.cshtml");
            }
            return View("~/Views/Admin/AddMachine.cshtml");
        }
        public IActionResult DeleteMachine(int id)
        {
            if (id != 0)
            {
                OtherService.MachinelDelete(id);
                Machines = OtherService.MachineList();
                ViewBag.MachineList = Machines;
            }
            return View("~/Views/Admin/ListMachine.cshtml");
        }
        public IActionResult ListMachine()
        {
            Machines = OtherService.MachineList();
            ViewBag.MachineList = Machines;
            return View("~/Views/Admin/ListMachine.cshtml");
        }
        public IActionResult AddRawMaterial(string RawMaterialName, string RawMaterialSerialNo, double RawMaterialCap, int RawMaterialBoy)
        {
            if (!string.IsNullOrEmpty(RawMaterialName))
            {
                rawMaterial.HammaddeAdi = RawMaterialName;
                rawMaterial.HammaddeSeriNo = RawMaterialSerialNo;
                rawMaterial.HammaddeBoy = RawMaterialBoy;
                rawMaterial.HammaddeCap = RawMaterialCap;

                OtherService.rawMaterialAdd(rawMaterial);
                rawMaterials = OtherService.rawMaterialList();
                ViewBag.RawMaterialList = rawMaterials;
                return View("~/Views/Admin/ListRawMaterial.cshtml");
            }

            return View("~/Views/Admin/AddRawMaterial.cshtml");
        }
        public IActionResult ListRawMaterial()
        {
            rawMaterials = OtherService.rawMaterialList();
            ViewBag.RawMaterialList = rawMaterials;
            return View("~/Views/Admin/ListRawMaterial.cshtml");
        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return View("~/Views/Login/LoginView.cshtml");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult DeleteRawMaterial(int id)
        {
            if (id != 0)
            {
                OtherService.rawMaterialDelete(id);
                rawMaterials = OtherService.rawMaterialList();
                ViewBag.RawMaterialList = rawMaterials;
            }
            return View("~/Views/Admin/ListRawMaterial.cshtml");
        }
        public IActionResult AddCoatingInfo(string CoatName, DateTime CoatSevkTime, DateTime CoatTeslimTarihi)
        {
            if (!string.IsNullOrEmpty(CoatName))
            {
                coatingInfo.KaplamaAdi = CoatName;
                coatingInfo.SevkTarihi = CoatSevkTime;
                coatingInfo.TeslimTarihi = CoatTeslimTarihi;

                OtherService.CoatingInfoAdd(coatingInfo);
                coatingInfos = OtherService.CoatingInfoList();
                ViewBag.CoatingInfoList = coatingInfos;

                return View("~/Views/Admin/ListCoatingInfo.cshtml");
            }

            return View("~/Views/Admin/AddCoatingInfo.cshtml");
        }
        public IActionResult ListCoatingInfo()
        {
            coatingInfos = OtherService.CoatingInfoList();
            ViewBag.CoatingInfoList = coatingInfos;

            return View("~/Views/Admin/ListCoatingInfo.cshtml");
        }
        public IActionResult DeleteCoatingInfo(int id)
        {
            if (id != 0)
            {
                OtherService.CoatingInfoDelete(id);
                coatingInfos = OtherService.CoatingInfoList();
                ViewBag.CoatingInfoList = coatingInfos;
            }
            return View("~/Views/Admin/ListCoatingInfo.cshtml");
        }
    }
}