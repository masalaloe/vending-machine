using System;
using System.Collections.Generic;

public class TheMachine
{
	private static Dictionary<string, int> priceItem = new Dictionary<string, int>()
	{
		{"Biskuit", 6000},
		{"Chips", 8000},
		{"Oreo", 10000},
		{"Tango", 12000},
		{"Cokelat", 15000}
	};

	private static Dictionary<string, int> stockItem = new Dictionary<string, int>()
	{
		{"Biskuit", 1},
		{"Chips", 1},
		{"Oreo", 0},
		{"Tango", 4},
		{"Cokelat", 3}
	};

	private static List<int> acceptedMoney = new List<int>()
	{
		2000, 5000, 10000, 20000, 50000
	};

	private List<int> changeMoney = new List<int>();

	private int currentMoney = 0;

	public enum Menu
	{
		Biskuit = 1,
		Chips = 2,
		Oreo = 3,
		Tango = 4,
		Cokelat = 5
	}
	private void updateStock(string produk)
	{
		stockItem[produk] -= 1;
	}
	public int getStock(string produk)
	{
		return stockItem[produk];
	}
	public bool addMoney(int x)
	{
		try
		{
			foreach (var item in acceptedMoney)
			{
				if (x == item)
				{
					currentMoney = currentMoney + x;
					return true;
				}
			}
			return false;
			
		}
		catch (Exception)
		{
			return false;
		}
		
	}
	public int getMoney()
	{
		return currentMoney;
	}
	public List<int> getChange()
	{
		var i = Change();
		return changeMoney;
	}
	public void RestockSnack()
	{
		foreach (var item in stockItem)
		{
			if (item.Value == 0)
			{
				stockItem[item.Key] += 10;
				break;
			}
		}
	}
	public string Buy(string snack)
	{
		try
		{
			var stok = getStock(snack);
			if (stok == 0) return "Out of stock";
			if (currentMoney - priceItem[snack] < 0) return "Out of saldo.";

			currentMoney -= priceItem[snack];
			updateStock(snack);
			return "done.";
		}
		catch (Exception)
		{

			return "Produk undefined.";
		}
		
	}
	public int Change()
	{
		if (currentMoney == 0) return currentMoney;
		//20.000 - 12.000 = 8.0000
		//50.000 - 12.000 = 38.000
		changeMoney = new List<int>();
		int tempMoney = currentMoney;
		//2000, 5000, 10000, 20000, 50000.
		for (var i = acceptedMoney.Count - 1; i >= 0; i--)
		{

			if (tempMoney >= acceptedMoney[i]) //8k >= 5k
			{
				changeMoney.Add(acceptedMoney[i]);
				tempMoney -= acceptedMoney[i]; //3k
			}
		}
		if (tempMoney != 0) changeMoney.Add(tempMoney);
		tempMoney = 0;
		currentMoney = 0;

		return currentMoney;
	}
}

public class Program
{
	static void SaldoInput(TheMachine obj)
	{
		Console.Write("\nBerapa nominal yang ingin anda masukan (2000, 5000, 10000, 20000 atau 50000): ");
		try
		{
			var b = obj.addMoney(Int32.Parse(Console.ReadLine()));
			if (!b) Console.WriteLine("Maaf input yang anda masukan salah.\nCoba gunakan uang pecahan yang sesuai.");
		}
		catch (Exception)
		{
			Console.WriteLine("Maaf input yang anda masukan salah.\nCoba gunakan uang pecahan yang sesuai.");
		}
	}
	public static void printMachine(TheMachine obt)
	{
		Console.WriteLine("+------------------------------------+");
		Console.WriteLine("+         Vending Machine            +");
		Console.WriteLine("+--+---------------------------------+");
		Console.WriteLine($"|  |* Biskuit * @Rp.6000  * Stock: {obt.getStock("Biskuit")}   ");
		Console.WriteLine($"|  |* Chips   * @Rp.8000  * Stock: {obt.getStock("Chips")}     ");
		Console.WriteLine($"|  |* Oreo    * @Rp.10000 * Stock: {obt.getStock("Oreo")}      ");
		Console.WriteLine($"|  |* Tango   * @Rp.12000 * Stock: {obt.getStock("Tango")}     ");
		Console.WriteLine($"|  |* Cokelat * @Rp.15000 * Stock: {obt.getStock("Cokelat")}   ");
		Console.WriteLine("+------------------------------------+");
		Console.WriteLine($"Saldo: {obt.getMoney()}");
		Console.WriteLine("+------------------------------------+");
	}

	public static void printMenu()
	{
		Console.WriteLine("Available option: ");
		Console.WriteLine("1. Insert money.");
		Console.WriteLine("2. Choose food.");
		Console.WriteLine("3. Take change.");
		Console.WriteLine("\n99. Restock snack.");
	}

	public static void printSnackMenu()
	{
		Console.WriteLine("Available option: ");
		Console.WriteLine("1. Biskuit.");
		Console.WriteLine("2. Chips.");
		Console.WriteLine("3. Oreo.");
		Console.WriteLine("4. Tango.");
		Console.WriteLine("5. Cokelat.");
	}

	public static void printKembalian(TheMachine obj)
	{
		Console.WriteLine($"Uang kembalian senilai: {obj.getMoney()}");
		Console.WriteLine("Dengan rincian:");
		foreach (var item in obj.getChange())
		{
			Console.Write($"{item}, ");
		}
	}

	public static int printPilihan()
	{
		Console.Write("\nOption : ");
		try
		{
			return Int32.Parse(Console.ReadLine());
		}
		catch (Exception)
		{
			Console.WriteLine("Option not found.");
			return 0;
		}
	}

	enum NavMenu : int
	{
		Begin = 0,
		InsertMoney = 1,
		ChooseFood = 2,
		TakeChange = 3,
		RestockSnack = 99
	}

	public static void Main()
	{
		//1. Masukan uang
		//2. Pilih makanan
		//3. Makanan keluar
		//4. pilih_lagi? -> no. 2 / keluar?
		//5. Keluar kembalian

		var obj = new TheMachine();
		var menu = NavMenu.Begin;
		while (true)
		{
			printMachine(obj);

			switch (menu)
			{
				case NavMenu.Begin:
					printMenu();
					menu = (NavMenu)printPilihan();
					break;
				case NavMenu.InsertMoney:
					SaldoInput(obj);
					menu = NavMenu.Begin;
					break;
				case NavMenu.ChooseFood:
					printSnackMenu();
					var g = (TheMachine.Menu)printPilihan();
					Console.WriteLine(obj.Buy(g.ToString()));
					menu = NavMenu.Begin;
					break;
				case NavMenu.TakeChange:
					printKembalian(obj);
					menu = NavMenu.Begin;
					break;
				case NavMenu.RestockSnack:
					obj.RestockSnack();
					menu = NavMenu.Begin;
					break;
				default:
					Console.WriteLine("Option not found.");
					menu = NavMenu.Begin;
					break;
			}


			//Console.WriteLine($"-------------------- Saldo : {t.ShowSaldo()} --------------------");
			//if (!step1) step1 = MoneyInput(t);
			Console.ReadLine();
			Console.Clear();
		}
		
		


	}
}