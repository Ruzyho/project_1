/**
 * @File Nodes.cs
 * @Brief Contains the Node classes for use with Driver.cs
 * 
 * @Author Forest Thomas
 * @Date Fall 2015
 */

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace Nodes
{
	/**
	 * @Class Node_A
	 * Reads from confA.txt and creates a socket to communicate
	 * with node B
	 */ 
	public class Node_A
	{
		String path_name;
		public Node_A (String path)
		{
			path_name = path;
		}

		public void Start()
		{
			StreamReader sr = new StreamReader (path_name);
			String msg = sr.ReadLine ();
			int port = Int32.Parse (msg);

			TcpClient client = new TcpClient ();
			try {
				client.Connect (IPAddress.Parse("127.0.0.1"), port);
			} catch (System.Net.Sockets.SocketException e) {
				System.Console.WriteLine (e.Message);
			}
			NetworkStream stream = client.GetStream ();
			byte[] toBytes;
			msg = sr.ReadToEnd ();
			toBytes = Encoding.ASCII.GetBytes (msg);
			stream.Write (toBytes, 0, toBytes.Length);
			client.Close ();

		}
	}

	/**
	 * @Class Node_B
	 * Node that reads from confB.txt, listens to a port to 
	 * recieve data from node A, prints the data, then connects
	 * to node C and sends data from confB.txt
	 */ 
	public class Node_B
	{
		String path_name;
		public Node_B (String path)
		{
			path_name = path;
		}

		public void Start()
		{
			StreamReader sr = new StreamReader (path_name);
			String msg = sr.ReadLine ();
			int list_port = Int32.Parse (msg);
			msg = sr.ReadLine ();
			int tx_port = Int32.Parse (msg);

			/* listening to node A */
			TcpListener listener = new TcpListener (IPAddress.Parse("127.0.0.1"), list_port);
			listener.Start ();
			TcpClient client = listener.AcceptTcpClient ();
			NetworkStream stream = client.GetStream ();
			int i;
			Byte[] bytes = new Byte[256];
			System.Console.WriteLine ("Node B recieved: ");
			while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) {
				msg = System.Text.Encoding.ASCII.GetString (bytes, 0, i);
				System.Console.Write (msg);
			}
			System.Console.Write ("\n");
			client.Close ();

			/* Transmitting to node C */
			client = new TcpClient ();
			try {
				client.Connect (IPAddress.Parse("127.0.0.1"), tx_port);
			} catch (System.Net.Sockets.SocketException e) {
				System.Console.WriteLine (e.Message);
			}
			stream = client.GetStream ();
			byte[] toBytes;
			msg = sr.ReadToEnd ();
			toBytes = Encoding.ASCII.GetBytes (msg);
			stream.Write (toBytes, 0, toBytes.Length);
			client.Close ();
		}
	}

	/**
	 * @Class Node_C
	 * Recieves data from Node B and prints
	 * the data to the screen
	 */ 
	public class Node_C
	{
		String path_name;
		public Node_C (String path)
		{
			path_name = path;
		}

		public void Start()
		{
			StreamReader sr = new StreamReader (path_name);
			String msg = sr.ReadLine ();
			int port = Int32.Parse (msg);

			/* Listening to node B */
			TcpListener listener = new TcpListener (IPAddress.Parse("127.0.0.1"), port);
			listener.Start ();
			TcpClient client = listener.AcceptTcpClient ();
			NetworkStream stream = client.GetStream ();
			int i;
			Byte[] bytes = new Byte[256];
			System.Console.WriteLine ("Node C recieved: ");
			while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) {
				msg = System.Text.Encoding.ASCII.GetString (bytes, 0, i);
				System.Console.Write (msg);
			}
			System.Console.Write ("\n");
			client.Close ();
		}
	}
}