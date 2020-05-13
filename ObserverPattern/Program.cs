using System;
using System.Collections.Generic;

namespace ObserverPattern
{
    public interface IObserver
    {
        // receive an update from the subject
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

    /// <summary>
    /// This is our Store class that implements ISubject that we want to subscribe to notifications
    /// </summary>
    public class Store : ISubject
    {
        public int inventoryCount = 0;
        public List<string> inventory = new List<string>();
        private List<IObserver> observers = new List<IObserver>();

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
    class StoreAssistant : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Store).inventoryCount > 3)
            {
                Console.WriteLine("StoreAssistant: Reacted to an event.");
            }
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

            // Instantiate our assistant to monitor the store for changes
            StoreAssistant assistant = new StoreAssistant();
            store.Attach(assistant);
            store.Add("iPhone SE (2020)");
            Console.ReadLine();
        }
    }
}
