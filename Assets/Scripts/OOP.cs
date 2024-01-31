using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOP : MonoBehaviour
{
    private void Start()
    {
        Animal dog1 = new Dog("Tuzik");
        Animal dog2 = new Dog("Bobik");
        dog2.MakeSound();
        Dog.PrintHowManyDogs();
        Dog dog = new Dog("Rex");
        dog.MakeSound();
        Dog.PrintHowManyDogs();
        Duck duck = new Duck("Donald");
        duck.MakeSound();
        duck.Swim();
    }

}

public abstract class Animal
{
    protected string name;
    public Animal(string name)
    { 
    this.name = name;   
    }

    public virtual void MakeSound()
    {
        Debug.Log($"{name} makes a sound.");
    }
}
   
public class Dog : Animal
{
    public static int dogCount = 0;

    public Dog(string name) : base(name) 
    {
        dogCount++;
    }

    public override void MakeSound() 
    {
        Debug.Log($"{name} barks.");
    }

    public static void PrintHowManyDogs()
    {
        Debug.Log($"There are {dogCount} dogs.");
    }
}

public interface ISwimming
{
    void Swim();
}

public class Duck : Animal, ISwimming
{
    public Duck(string name) : base(name) { }

    public override void MakeSound()
    {
        Debug.Log($"{name} quacks.");
    }

    public void Swim()
    {
        Debug.Log($"{name} swims.");
    }

}
class Program
{
    static void Main(string[] args)
    {
        Dog dog = new Dog("Rex");
        dog.MakeSound();
        Dog.PrintHowManyDogs();

        Duck duck = new Duck("Donald");
        duck.MakeSound();
        duck.Swim();
    }
    
}