using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstProgram : MonoBehaviour
{
    struct MyStruct
    {
        public int X;
        public int Y;
    }
    // Start is called before the first frame update
    void Start()
    {
        #region Задание 1

        /*1. Создайте переменные следующих типов данных:
         * int, float, bool, string. 
         * Назовите их myInt, myFloat, myBool, myString соответственно.
         * Также создайте константы с теми же типами данных и 
         * назовите их myConstInt, myConstFloat, myConstBool, myConstString.
         * Напишите однострочный и многострочный комментарии рядом с этими переменными.
        */

        int myInt = 10; 
        float myFloat = 20.5585f; 
        bool myBool = true; 
        string myString = "Answer to the Ultimate Question of Life, The Universe, and Everything?";
        // Однострочный комментарий
        const int myConstInt = 42;
        const float myConstFloat = 200.5f;
        const bool myConstBool = false;
        const string myConstString = " Hello, Universe!";
        /* 
         * Многострочный комментарий
       */
        Debug.Log(myString);
        Debug.Log(myConstInt);


        #region Ответ
        /*
        // Однострочный комментарий
        int myInt = 10; // Объявление переменной типа int
        float myFloat = 20.5f; // Объявление переменной типа float
        bool myBool = true; // Объявление переменной типа bool
        string myString = "Hello, World!"; // Объявление переменной типа string
        */
        /* Многострочный комментарий
           Объявление констант 
        const int myConstInt = 100;
        const float myConstFloat = 200.5f;
        const bool myConstBool = false;
        const string myConstString = "Hello, Universe!";
        */

        #endregion

        #endregion

        #region Задание 2

        /*2. Используйте созданные переменные и операторы +, -.
         * Выполните сложение переменной myInt с myConstInt. Сохраните результат в переменную типа int, по имени resultInt.
         * Выполните вычитание переменной myConstFloat из переменной myFloat. Сохраните результат в переменную типа float, по имени resultFloat.
         * Выполните сложение (конкатенацию) строковых переменных myString и myConstString. Сохраните результат в переменную типа string, по имени resultString.
        */
        int resultInt = myInt + myConstInt;
        float resultFloat = myConstFloat - myFloat;
        string resultString = myString + myConstString;
        Debug.Log(resultInt);
        Debug.Log(resultFloat);
        Debug.Log(resultString);

        #region Ответ
        /*
        int resultInt = myInt + myConstInt; // Сложение int
        float resultFloat = myFloat - myConstFloat; // Вычитание float
        string resultString = myString + myConstString; // Конкатенация строк
        */
        #endregion

        #endregion

        #region Задание 3

        /*3. Используйте математическую функцию Math.Sqrt на переменной myInt. Сохраните результат в переменную типа double под названием sqrtResult.
         * Используйте математическую функцию Math.Pow на переменной myFloat, возведите myFloat во вторую степень. Сохраните результат в переменную типа double под названием powResult.
        */
        double sqrtResult = Mathf.Sqrt(myInt);
        double powResult = Mathf.Pow(myFloat, 2);
        Debug.Log(sqrtResult);
        Debug.Log(powResult);

        #region Ответ
        /*
        double sqrtResult = Math.Sqrt(myInt); // Корень квадратный
        double powResult = Math.Pow(myFloat, 2); // Возведение в степень
        */
        #endregion

        #endregion

        #region Задание 4

        /*4. Произведите явное и неявное преобразование типов данных с выводом значений в консоль.
         * Объявите переменную типа double под названием myDouble и сделайте неявное преобразование переменной myInt. Выведите в консоль результат.
         * Объявите переменную типа int под названием myNewInt и сделайте явное преобразование переменной myFloat к типу int. Выведите в консоль результат.
        */
        double myDouble = myInt; 
        Debug.Log(myDouble);
        int myNewInt = (int)(myFloat);
        Debug.Log(myNewInt);

        #region Ответ
        /*
        // Неявное преобразование
        double myDouble = myInt;
        Debug.Log(myDouble);

        // Явное преобразование
        int myNewInt = (int)myFloat;
        Debug.Log(myNewInt);
        */
        #endregion

        #endregion

        #region Задание 5

        //5. Объявите массив под именем myArray и с типом данных string, инициализируйте его любым количеством строковых данных. Используйте элементы массива для вывода в консоль.
        string[] myArray;
        myArray = new string[5];
        myArray[0] = "Half";
        myArray[1] = "Life";
        myArray[2] = "Three";
        Debug.Log(myArray[0]);
        Debug.Log(myArray[1]);
        Debug.Log(myArray[2]);
        #region Ответ
        /*
        string[] myArray = new string[3] {"One", "Two", "Three"};
        Debug.Log(myArray[0]);
        Debug.Log(myArray[1]);
        Debug.Log(myArray[2]);
        */
        #endregion

        #endregion

        #region Задание 6

        /*6. Объявите лист под именем myList с типом данных int, добавьте в него цифры 1, 2, 3, используя методы Add. 
         * Выведите в консоль значения элементов списка. 
         * Удалите любые элементы используя функцию Remove, RemoveAt.
        */
         List<int> myList = new List<int>();
        myList.Add(1);
        myList.Add(2);
        myList.Add(3);
        Debug.Log(myList[0]);
        Debug.Log(myList[1]);
        Debug.Log(myList[2]);
        myList.Remove(2);
        myList.RemoveAt(0);
        #region Ответ
        /*
        List<int> myList = new List<int>();
        myList.Add(1);
        myList.Add(2);
        myList.Add(3);
        Debug.Log(myList[0]);
        Debug.Log(myList[1]);
        Debug.Log(myList[2]);
        myList.Remove(2);
        myList.RemoveAt(0);
        */
        #endregion

        #endregion

        #region Задание 7

        /*7. Создайте структуру под именем MyStruct. Добавьте в структуру переменные типа int под именем X и именем Y.
         * Создайте переменную с типом структуры MyStruct и именем myStruct.
         * Проинициализируйте переменную myStruct, укажите значения переменным X и Y.
         * Выведите в консоль значения переменных X и Y из структуры myStruct.
        */
        

    MyStruct myStruct;
    myStruct.X = 20;
        myStruct.Y = 17;
        Debug.Log(myStruct.X);
        Debug.Log(myStruct.Y);
        #region Ответ
        /*
        struct MyStruct
        {
            public int X;
            public int Y;
        }

        MyStruct myStruct;
        myStruct.X = 10;
        myStruct.Y = 20;
        Debug.Log(myStruct.X);
        Debug.Log(myStruct.Y);
        */
        #endregion

        #endregion

        #region Задание 8

        //8. Создайте условие в котором проверяете, больше ли значение переменной myInt чем 0. Используйте if else и выведите в консоль разные исходы.
        if (myInt > 0)
        {
            Debug.Log("myInt>0");
        }
        else
        {
            Debug.Log("myInt<0");
        }
        #region Ответ
        /*
        if (myInt > 0)
        {
            Debug.Log("myInt is positive.");
        }
        else
        {
            Debug.Log("myInt is not positive.");
        }
        */
        #endregion

        #endregion

        #region Задание 9

        //9. Используйте switch для проверки что находится в переменной myString. Используйте несколько case и default. Выведите в консоль разные исходы.
        switch (myString)
        {
            case "Answer to the Ultimate Question of Life, The Universe, and Everything?":
                Debug.Log("42");
                break;
            case " Hello, Universe!":
                Debug.Log("Greeting the aliens.");
                break;
            default:
                Debug.Log("Unknown greeting.");
                break;
        }
        #region Ответ
        /*
        switch (myString)
        {
            case "Hello, World!":
                Debug.Log("Greeting the world.");
                break;
            case "Hello, Universe!":
                Debug.Log("Greeting the universe.");
                break;
            default:
                Debug.Log("Unknown greeting.");
                break;
        }
        */
        #endregion

        #endregion

        #region Задание 10

        /* 10. Используйте цикл for для вывода всех значений массива myArray.
         * Используйте цикл while для удаления всех элементов списка myList с помощью метода RemoveAt(0).
         * Установите значение переменной myInt равным 10. Используйте цикл do while для того чтобы снизить значение переменной myInt до 0. Выводите на каждом шаге в консоль значение myInt. 
        */
        for (int i = 0; i < myArray.Length; i++)
        {
            Debug.Log(myArray[i]); // Вывод элементов массива 
        }
        while (myList.Count > 0)
        {
            Debug.Log(myList[0]); // Вывод первого элемента списка 
            myList.RemoveAt(0); // Удаление первого элемента списка 
        }
        do
        {
            Debug.Log(myInt); // Вывод значения myInt 
            myInt--; // Уменьшение значения myInt на 1 
        } while (myInt > 0);
        #region Ответ
        /*
        for (int i = 0; i < myArray.Length; i++) { 
            Debug.Log(myArray[i]); // Вывод элементов массива 
        } 
        while (myList.Count > 0) { 
            Debug.Log(myList[0]); // Вывод первого элемента списка 
            myList.RemoveAt(0); // Удаление первого элемента списка 
        } 
        do { 
            Debug.Log(myInt); // Вывод значения myInt 
            myInt--; // Уменьшение значения myInt на 1 
        } while (myInt > 0); 
        */
        #endregion

        #endregion

        #region Задание 11

        //11. Создайте функцию без возвращающего значения и без параметров с именем MyFunction. Выведете в теле этой функции любую информацию в консоль. Вызовете эту функцию из любого места.
        void MyFunction()
        {
            Debug.Log("Function's working."); 
        }
        MyFunction();
        #region Ответ
        /*
        void MyFunction() { 
            Debug.Log("This is my function."); // Вывод сообщения 
        } 
        MyFunction(); // Вызов функции 
        */
        #endregion

        #endregion

        #region Задание 12

        //12. Создайте функцию без возвращающего значения с параметрами с именем MyFunctionWithParameters. Параметры назовите param1 и param2. param1 это целочисленный тип, а param2 - строковый. Выведите в консоль значения параметров в теле этой функции. Вызовете эту функцию из любого места.
        void MyFunctionWithParameters(int param1, string param2)
        {
            Debug.Log("This is my function with parameters: " + param1 + ", " + param2);
        }
        MyFunctionWithParameters(myInt, myString);
        #region Ответ
        /*
        void MyFunctionWithParameters(int param1, string param2) { 
            Debug.Log("This is my function with parameters: " + param1 + ", " + param2); // Вывод сообщения с параметрами 
        } 
        MyFunctionWithParameters(myInt, myString); // Вызов функции с параметрами
        */
        #endregion

        #endregion

        #region Задание 13
        //13. Создайте функцию с возвращающим значением с параметрами с именем MyFunctionWithReturnValueAndParameters. Параметры назовите param1 и param2. Оба параметра целочисленные. Верните с помощью return сумму этих переменных.
        int MyFunctionWithReturnValueAndParameters(int param1, int param2)
        {
            return param1 + param2; 
        }
        int result = MyFunctionWithReturnValueAndParameters(myInt, myConstInt);
        Debug.Log(result); 
        #region Ответ
        /*
        int MyFunctionWithReturnValueAndParameters(int param1, int param2) { 
            return param1 + param2; // Возврат суммы параметров 
        } 
        int result = MyFunctionWithReturnValueAndParameters(myInt, myConstInt); // Вызов функции с параметрами и сохранение результата 
        Debug.Log(result); // Вывод результата
        */
        #endregion
        #endregion
    }

}
