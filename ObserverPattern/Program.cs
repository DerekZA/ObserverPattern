using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http.Headers;

namespace ObserverPattern
{
    public interface IObserver
    {
        // receive an update from the subject, and notify the subscriber
        void Update(ISubject subject);
    }

    public interface ISubject
    {
        // Attach an observer to the subject
        void Attach(IObserver observer);
        // Detach an observer from the subject
        void Detach(IObserver observer);
        // Notifiy all observers there was a change
        void Notify();
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
        public int inventoryCount = 0;
        public List<string> inventory = new List<string>();
        private readonly List<IObserver> observers = new List<IObserver>();

        /// <summary>
        /// This adds a new product to the store
        /// </summary>
        /// <param name="product"></param>
        public void Add(string product)
        {
            inventory.Add(product);
            Console.WriteLine(string.Format("{0} has been added to the store", product));
            inventoryCount += 1;
            Notify();
        }
        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Attached an observer");
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
            Console.WriteLine("Subject: Detached an observer");
        }

        public void Notify()
        {
            Console.WriteLine("Notifying all observers...");
            foreach (var observer in observers)
            {
                observer.Update(this);
            }
        }
    }

    /// <summary>
    /// This is the observer that will attach the the Store class for updates to it's inventory
    /// </summary>
    public class StoreAssistant : IObserver
    {
        readonly string product;
        readonly ISubscriber customer;
        public StoreAssistant(string productInterest, ISubscriber subscriber)
        {
            product = productInterest;
            this.customer = subscriber;
        }
        public void Update(ISubject subject)
        {
            if ((subject as Store).inventory.Contains(product))
            {
                Console.WriteLine("StoreAssistant: Reacted to an event.");
                customer.Notify();

            }
        }
    }

    public class Customer : ISubscriber
    {
        public string Mobile { get; set; }
        public string ProductInterest { get; set; }

        public void Notify()
        {
            Console.WriteLine(string.Format("Sending notification to {0} that the new {1} is now available", Mobile, ProductInterest));
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
        static void Main(string[] args)
        {
            // Instantiate our store with some current inventory
            Store store = new Store();
            store.inventory.Add("iPhone X");
            store.inventoryCount += 1;
            store.inventory.Add("iPhone 11");
            store.inventoryCount += 1;
            store.inventory.Add("iPhone 11 Pro");
            store.inventoryCount += 1;

            // Here comes our first customer
            Customer customer = new Customer();
            Console.Write("What product are you intereted in? ");
            customer.ProductInterest = Console.ReadLine();
            Console.Write(string.Format("Enter your mobile and we will let you know when the {0} becomes available: ", customer.ProductInterest));
            customer.Mobile = Console.ReadLine();
            customer.SubscribeForNotification(store, customer.ProductInterest);

            // The customers product becomes available in the store
            store.Add(customer.ProductInterest);
            Console.ReadLine();
        }
    }
}
