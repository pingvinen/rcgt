require 'socket'               # Get sockets from stdlib

server = TCPServer.open(2000)  # Socket to listen on port 2000
loop {                         # Servers run forever
	client = server.accept       # Wait for a client to connect
	#client.puts(Time.now.ctime)  # Send the time to the client
	#client.puts "Closing the connection. Bye!"
	#client.close                 # Disconnect from the client
	begin
		while received = client.readline do
			received.strip!
			next if received.length == 0
			puts("'#{received}'")
		end
	rescue EOFError => err
		# do not die... just wait for new connection
	end
}
