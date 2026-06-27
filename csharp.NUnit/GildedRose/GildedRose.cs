using System;
using System.Collections.Generic;

namespace GildedRoseKata;


/*
 * 
 * 
 * 1. Understand the Rules
 * What is really needed ?
 * 
 * 2. Understand the Code
 * In order to understand the code and see if everything works
 * as expected my approach will be, Write test first that test the logic of the Business Rules. 
 * 
 * So my approach was write the business rules like a "checklist" in a paper go and check one rule
 * at a time with a test and repeat. 
 * 
 * This wil help alot to understand what is actually happening in the Code
 * and also I will be able to find Bugs in the System, if any.
 * 
 * 
 * 3. Now that I have all tests Cases that cover the needed Business Rules,
 * 
 *    - First before changing the messy code I will try to add conjure items logic and make the test pass.
 *    
 *    - Then I will make small changes to the if statments, like try to separate the 
 *    if statements to own Items Categories this will allow me to make each logic dependent and 
 *    a bit more readable. 
 *    
 * 4. Now its time to maybe create an ItemObject with category methods or Objects that inherit form Item 
      each Item category with its own rules
 * in GRASP Principle, this would be Information Expert, add the rules to the Object
 * of the same context or Dependency Injection hmmmmmmmmmmmmmm
 * 
 * Ok Dependency Injection would be here a good Option. I will go for dependency Injection
 * 
 * 
 * 
 */



// Category Classes
public static class ItemCategory
{
    public static readonly string Sulfuras = "Sulfuras, Hand of Ragnaros";
    public static readonly string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
}

public interface IItemUpdateRule
{
    public bool AppliesTo(string itemName);
    public void Update(Item item);
}


public abstract class BaseItemRules
{
    protected int MinQualityValue { get; } = 0;
    protected int MaxQualityValue { get; } = 50;
    protected int SellInDay { get;  } = 0;


    // SellinRules
    protected void SellInDropsByOneDay(Item item)
    {
        item.SellIn = item.SellIn - 1;
    }

    // QualityRules
    protected bool QualityIsNegativeOrCero(int itemQuality)
    {
       return itemQuality <= MinQualityValue;
    }

    protected void QualityCanNotBeHigherThanFifty(Item item)
    {
        if (item.Quality >= MaxQualityValue) item.Quality = MaxQualityValue;
    }


    protected void QualityRiseByOne(Item item)
    {
        item.Quality = item.Quality + 1;
    }

    protected void QualityRiseByTwo(Item item)
    {
        item.Quality = item.Quality + 2;
    }

    protected void QualityDropsByOne(Item item)
    {
        item.Quality = item.Quality - 1;
    }
    protected void QualityDropsByTwo(Item item)
    {
        item.Quality = item.Quality - 2;
    }

}
public class NormalUpdateRule : BaseItemRules, IItemUpdateRule
{
    public bool AppliesTo(string itemName)
    {
        return true;
    }

    public void Update(Item item)
    {
        // SellInRules
        base.SellInDropsByOneDay(item);
        
        //  Rules
        if (base.QualityIsNegativeOrCero(item.Quality)) item.Quality = base.MinQualityValue;
        else
        {
            if (item.SellIn < base.SellInDay) base.QualityDropsByTwo(item);
            else base.QualityDropsByOne(item);
        }
        // Quality Rule
        base.QualityCanNotBeHigherThanFifty(item);
    }
}

public class ConjuredUpdateRule : BaseItemRules, IItemUpdateRule
{

    public bool AppliesTo(string itemName)
    {
        return itemName == "Conjured Mana Cake";
    }

    public void Update(Item item)
    {
        // SellInRules
        base.SellInDropsByOneDay(item);

        // Rules
        if (base.QualityIsNegativeOrCero(item.Quality)) item.Quality = base.MinQualityValue;
        else base.QualityDropsByTwo(item);

        // Quality Rule
        base.QualityCanNotBeHigherThanFifty(item);
    }
}

public class AgedBrieUpdateRule : BaseItemRules, IItemUpdateRule
{

    public bool AppliesTo(string itemName)
    {
        return itemName == "Aged Brie";
    }

    public void Update(Item item)
    {
        // SellInRules
        base.SellInDropsByOneDay(item);

        // Rules
        if (base.QualityIsNegativeOrCero(item.Quality)) item.Quality = base.MinQualityValue;

        if (item.SellIn < base.SellInDay) QualityRiseByTwo(item);
        else QualityRiseByOne(item);

        // Quality Rule
        base.QualityCanNotBeHigherThanFifty(item);
    }
}

public class GildedRose
{
    IList<Item> Items;
    IList<IItemUpdateRule> Rules;

    public GildedRose(IList<Item> Items, IList<IItemUpdateRule> Rules)
    {
        this.Items = Items;
        this.Rules = Rules;
    }


    public Item GetItemByIndex(int index)
    {
        return Items[index];
    }

    

    public void UpdateQuality()
    {

        for (var i = 0; i < Items.Count; i++)
        {


            // Item Properties
            string itemName = Items[i].Name;
            int itemSellIn = Items[i].SellIn;
            int itemQuality = Items[i].Quality;

            // Item Rules
            int MinSellInValue = 0;
            int MinQualityValue = 0;
            int MaxQualityValue = 50;

            // Category: Sulfuras

            if (itemName == ItemCategory.Sulfuras)
            {
                // Quality Rules
                Items[i].Quality = 80;
                
                continue;
            }

            // Category: Backstage passes

            if (itemName == ItemCategory.BackstagePasses)
            {

                //SellIn Rules
                Items[i].SellIn = itemSellIn - 1;

                // Quality Rules

                if (itemQuality <= MinQualityValue) Items[i].Quality = MinQualityValue;

                if (itemSellIn <= MinSellInValue) Items[i].Quality = MinQualityValue;
                else if (itemQuality < MaxQualityValue)
                {
                    if (itemSellIn <= 5) Items[i].Quality = itemQuality + 3;
                    else if (itemSellIn <= 10) Items[i].Quality = itemQuality + 2;
                    else Items[i].Quality = itemQuality + 1;
                }

                if (Items[i].Quality > MaxQualityValue) Items[i].Quality = MaxQualityValue;

                continue;
            }

            foreach (IItemUpdateRule itemRule in Rules)
            {
                if (itemRule.AppliesTo(Items[i].Name))
                {
                    itemRule.Update(Items[i]);
                    break;
                }
            }

        }
    }
}