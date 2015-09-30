/**
 * @File Driver.cs
 * @Brief A program the sends data between nodes using sockets
 * 
 * @Author Forest Thomas
 * @Date Fall 2015
 */ 
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Nodes;

namespace Application
{
	public class Driver
	{
		public static void Main ()
		{
			String path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

			/* Establishing file names */
			String confA, confB, confC;
			confA = path + "/confA.txt";
			confB = path + "/confB.txt";
			confC = path + "/confC.txt";

			/* Checking if files exist */
			if (!File.Exists(confA))
			{
				System.Console.WriteLine ("Failed to find confA.txt");
				return;
			}
			if (!File.Exists(confB))
			{
				System.Console.WriteLine ("Failed to find confB.txt");
				return;
			}
			if (!File.Exists(confC))
			{
				System.Console.WriteLine ("Failed to find confC.txt");
				return;
			}

			/* Instantiating nodes */
			Node_A a = new Nodes.Node_A (path + "/confA.txt");
			Node_B b = new Nodes.Node_B (path + "/confB.txt");
			Node_C c = new Nodes.Node_C (path + "/confC.txt");

			/* Instantiate threads */
			Thread aThread = new Thread (new ThreadStart(a.Start));
			Thread bThread = new Thread (new ThreadStart(b.Start));
			Thread cThread = new Thread (new ThreadStart(c.Start));

			/* Start Threads */
			aThread.Start ();
			bThread.Start ();
			cThread.Start ();

			/* Wait for threads to finish */
			aThread.Join ();
			bThread.Join ();
			cThread.Join ();
			Console.WriteLine ("All threads have finished running");
		}
	}
}

