using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.ConstrainedExecution;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            //доступные методы для теста:
            //skladik.DriveToTheParkingLot - позволяет машине припарковаться, что делает возможной дальнейшую ее разгрузку
            //skladik.DisplayInfoAboutParkingLots - вывод всех припаркованных машин
            //worker1.UnloadWhileTrue - работник разгружает машину пока это возможно(пока есть места на складе и пока в машине есть что разгружать)
            //worker1.DisplayInfoAboutMachine - узнать сколько в машине неразгруженного веса(все написано в килограммах)
            //worker1.DisplayInfoAboutSklad - узнать насколько загружен склад



            MAN man1 = new MAN(100, "MAN1", 10000);
            KAMAZ kamazik = new KAMAZ();
            Worker worker1 = new Worker();
            Sklad skladik = new Sklad();

            //Проверка на занятость мест для паркинга
            skladik.DisplayInfoAboutParkingLots();

            //разгрузка, если это возможно и пока это возможно
            worker1.UnloadWhileTrue(man1, skladik);

        }
        public abstract class Car
        {
            public bool _isParking = false;
            public int _capacity { get; set; }
            public int _speed { get; set; }
            public string _title { get; set; }
        }

        //Man - марка камаза
        sealed class MAN : Car
        {
            public MAN()
            {
                _speed = 0;
                _title = "Undefined";
                _capacity = 0;
                _isParking = false;
            }
            public MAN(int speed, string title, int cap)
            {
                _speed = speed;
                _title = title;
                _capacity = cap;
                _isParking = false;
            }
        }
        sealed class KAMAZ : Car
        {
            public KAMAZ()
            {
                _speed = 0;
                _title = "Undefined";
                _capacity = 0;
                _isParking = false;
            }

            public KAMAZ(int speed, string title, int cap)
            {
                _speed = speed;
                _title = title;
                _capacity = cap;
                _isParking = true;
            }
        }
        class Sklad
        {
            static Worker worker = new Worker();

            static int maxParkingPlaces = 3;

            public static List<Car> placesForCars = new List<Car>();

            public static int howManyBoards = 10;

            public static int maxOnOneBoard = 1000;
            public int onSklNow { get; set; }

            public int maxOnSklad = howManyBoards * maxOnOneBoard;
            public void DriveToTheParkingLot(Car car)
            {
                if (placesForCars.Count < maxParkingPlaces)
                {
                    placesForCars.Add(car);
                    car._isParking = true;
                }
                else
                {
                    Console.WriteLine("Все места для парковки заняты, ожидайте когда место освободится");
                    car._isParking = false;
                }
            }
            public void DisplayInfoAboutParkingLots()
            {
                Console.WriteLine();
                Console.WriteLine("На складе сейчас припаркованы:");
                if (placesForCars.Count == 0)
                {
                    Console.WriteLine("0 машин");
                }
                foreach (var item in placesForCars)
                {
                    Console.WriteLine(item._title);
                }
                Console.WriteLine();
            }

        }
        class Worker
        {
            public static int nowCap = 30;
            protected void UnloadCar(Car car)
            {
                if (car._capacity - nowCap <= 0)
                {
                    nowCap = car._capacity;
                    car._capacity -= nowCap;
                }
                else
                {
                    car._capacity -= nowCap;
                }

            }
            protected void LoadToSklad(Sklad skl, Car car)
            {
                skl.onSklNow += nowCap;
                if (skl.onSklNow >= skl.maxOnSklad)
                {
                    skl.onSklNow = skl.maxOnSklad;
                }
            }
            public void UnloadWhileTrue(Car car, Sklad skl)
            {
                if (car._isParking == true)
                {
                    while (skl.maxOnSklad >= skl.onSklNow && car._capacity >= 0)
                    {

                        if (skl.onSklNow >= skl.maxOnSklad)
                        { break; }
                        else if (car._capacity < 0)
                        { break; }

                        UnloadCar(car);
                        LoadToSklad(skl, car);
                        DisplayInfoAboutMachine(car);
                        DisplayInfoAboutSklad(skl);
                    }
                }
                else
                {
                    Console.WriteLine($"Машина({car._title}) еще не припаркована на месте для разгрузки");

                }
            }

            public static void DisplayInfoAboutMachine(Car car)
            {
                Console.WriteLine($"В камазе осталось {car._capacity} килограмм\n");
            }
            public static void DisplayInfoAboutSklad(Sklad skl)
            {
                Console.WriteLine($"Склад загружен на {skl.onSklNow} килограмм из {skl.maxOnSklad}\n");
            }
        }
    }
}

