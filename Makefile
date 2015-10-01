CC=mcs

all: Driver

Driver: Driver.cs Nodes.cs
	$(CC) Driver.cs Nodes.cs

clean:
	rm Driver.exe
