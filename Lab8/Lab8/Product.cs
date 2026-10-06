using System;

public class Product
{
    private int id;
    private string name;
    private string category;
    private decimal price;
    private int quantity;
    private string manufacturer;

    public Product()
    {
    }

    public Product(int id, string name, string category, decimal price, int quantity, string manufacturer)
    {
        this.id = id;
        this.name = name;
        this.category = category;
        this.price = price;
        this.quantity = quantity;
        this.manufacturer = manufacturer;
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Category
    {
        get { return category; }
        set { category = value; }
    }

    public decimal Price
    {
        get { return price; }
        set { price = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    public string Manufacturer
    {
        get { return manufacturer; }
        set { manufacturer = value; }
    }

    public override string ToString()
    {
        return $"ID: {id}, Название: {name}, Категория: {category}, " +
               $"Цена: {price:F2}, Количество: {quantity}, Производитель: {manufacturer}";
    }
}