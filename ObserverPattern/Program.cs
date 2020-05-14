using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;

namespace ObserverPattern
{
    public interface IObserver
    {
        // process an update from the store
        void Update(string model);
    }

    public interface ISubject
    {
        // Attach an observer to the Store
        void Attach(IObserver observer);
        // Detach an observer from the Store
        void Detach(IObserver observer);
        // Notifiy all observers there is a new product in the Store
        void Notify(string product);
    }

    public interface ISubscriber
    {
        // Notify the subscriber
        void Notify();
    }

    /// <summary>
    /// This is our Store class that implements ISubject that we want to subscribe to notifications
    /// </summary>
    public class Store : ISubject
    {
        public List<string> inventory = new List<string>();
        private readonly List<IObserver> observers = new List<IObserver>();

        public Store()
        {
            inventory.Add("iPhone X");
            inventory.Add("iPhone 11");
            inventory.Add("iPhone 11 Pro");
        }

        /// <summary>
        /// This adds a new product to the store
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(string product)
        {
            inventory.Add(product);
            Console.WriteLine(string.Format("{0} has been added to the store", product));
            Notify(product);
        }
        public void Attach(IObserver observer)
        {
            Console.WriteLine("Store: Attached an observer");
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
            Console.WriteLine("Store: Detached an observer");
        }

        public void Notify(string product)
        {
            Console.WriteLine("Notifying all observers...");
            foreach (var observer in observers)
            {
                observer.Update(product);
            }
        }
    }

    /// <summary>
    /// This is the observer class, it will attach to the Store class for updates to it's inventory
    /// </summary>
    public class StoreAssistant : IObserver
    {
        readonly string product;
        readonly ISubscriber subscriber;
        public StoreAssistant(string productInterest, ISubscriber customer)
        {
            product = productInterest;
            subscriber = customer;
        }

        public void Update(string model)
        {
            if (model == product)
            {
                Console.WriteLine(string.Format("StoreAssistant: Reacted to {0} being added to the store.", model));
                subscriber.Notify();
            }
        }
    }

    public class Customer : ISubscriber
    {
        public string Name { get; set; }
        public string ProductInterest { get; set; }

        public void Notify()
        {
            // Here we implement logic to notify our customer
            Console.WriteLine(string.Format("Hi {0}, the new {1} is now available", Name, ProductInterest));
        }

        public void SubscribeForNotification(Store store, string product)
        {
            // Instantiate our assistant to monitor the store for changes
            StoreAssistant assistant = new StoreAssistant(product, this);
            store.Attach(assistant);
        }
    }
    class Program
    {
        static void Main()
        {
            // Instantiate our store with some current inventory
            Store store = new Store();

            // Here comes our first customer
            Customer customer = new Customer();
            Console.Write("Enter your name? ");
            customer.Name = Console.ReadLine();
            Console.Write("What product are you intereted in? ");
            customer.ProductInterest = Console.ReadLine();
            customer.SubscribeForNotification(store, customer.ProductInterest);

            // The customers product becomes available in the store
            store.AddProduct(customer.ProductInterest);
            Console.ReadLine();
        }
    }
}
