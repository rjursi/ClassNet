#include "client_socket_class.h"
#include <assert.h>
#include <time.h>
#include <Windows.h>


client_socket_class::client_socket_class() {
	WSAStartup(MAKEWORD(2, 2), &wsaData);

	/// ������ �����ϴ� Word ũ���, 0000001000000010 =  514
	/// wsaData : Window ���� ������ ���λ����� �����ϴ� WSADATA ������ ���� ������



	if ((sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP)) == -1) {
		temp = WSAGetLastError(); 
		printf("Socket Error: %x(%d)\n", temp, temp); 
		assert(sock != INVALID_SOCKET);

		/// ������ �������� �ʾ��� ���

	}

	//UDP �������� ����
		

	
	memset(&sock_addr, 0, sizeof(sock_addr));
	// �ش� ���� �ּ� ��ü�� 0���� �ʱ�ȭ

	sock_addr.sin_family = AF_INET; //IPv4


	getServerIP();

	// not a newline problem



	sock_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	sock_addr.sin_port = htons(PORT); //short ������
	
	// dst server IP and PORT NUMBER

	// htonl (Host to network long)
	// htons (Host to Network short)

	// sock_addr ��ü�� ����ҽ� �ʿ��� ����, ������ ���� ��ȯ�� �ʿ�
	// ��Ʈ��ũ������ �򿣵�� ����� ������ ���� ����� ���
	// ���� �ڵ� ȯ��(CPU) ������ ��Ʋ ����� ���, �� ��Ʈ��ũ ����� ����Ʈ ������ ������ߵ�

	bind(sock, (sockaddr*)&sock_addr, sizeof(sock_addr));
	// sock �������� �ش� ������ ���� ����

	join_addr.imr_interface.s_addr = inet_addr(serverIPAddress.c_str());
	join_addr.imr_multiaddr.s_addr = inet_addr("239.1.1.2");
	

	setsockopt(sock, IPPROTO_IP, IP_ADD_MEMBERSHIP, (char*) &join_addr, sizeof(join_addr));

	// set socket to multicast for join

	cout << "Wait for a response.";
}

// Ŭ���̾�Ʈ ��ü�� ������ �� �� �ٷ�		

client_socket_class::~client_socket_class() {
	//setsockopt(sock, IPPROTO_IP, IP_DROP_MEMBERSHIP, (char*)&join_addr, sizeof(join_addr));
	// Multicast Group Clean

	closesocket(sock);
	WSACleanup();
}

void client_socket_class::getServerIP() {
	cout << "������ ���� IP�� �Է��ϼ���.\n";

	getline(cin, serverIPAddress);


	
}

void client_socket_class::connect_server() {
	ZeroMemory(var_connect, sizeof(var_connect));
	// var_connect : ���� (char �� ����), ���ڿ��� �ޱ� ���� ���� (s)

	// �ش� ������ �ش� ũ�⸸ŭ �������, �� ������ �� ������ ���ֹ���

	recvfrom(sock, var_connect, sizeof(var_connect), 0, (SOCKADDR*)&sender_addr, &sender_addr_size);


	/// recvfrom : ������ �׷� ����, �ҽ� �ּ� ����
	/// sock �������� ������ �����͸� var_connect ���ۿ� �����ϰ� �ش� Ŭ���̾�Ʈ �ּ� (�ް� �ִ� ���� ����) ������ sender_addr ���� ����
		

	sendto(sock, var_connect, sizeof(var_connect), 0, (SOCKADDR*)&sender_addr, sock_addr_size);

	/// ������ ���� ���� �����͸� (var_connect ���ۿ� �ִ� �����͸�) sender_addr Ŭ���̾�Ʈ�� ����

	/// �� ���۸� ���� �ڽ�, �� Ŭ���̾�Ʈ�� �޴� ��

}

void client_socket_class::recvfile() {
	clock_t start, end;

	if (recvfrom(sock, (char*)&file_size, sizeof(file_size), 0, NULL, 0) == -1) {
		temp = WSAGetLastError();
		printf("Socket Error: %x(%d)\n", temp, temp);
	}


	//system("cls");
	cout << "���� ũ�� : " << file_size << " Byte" << endl;

	// ���� ��ü ũ���� ���� ����
	total_size = 0;
	totalbuf = new char[file_size];
	ZeroMemory(totalbuf, _msize(totalbuf));


	cout << "\nDownloading ";

	start = clock(); // �ش� ���μ����� �ý��ۿ��� �Һ��� �ð�
	
	// ���� ����
	buf = new char[BUFSIZE];


	while (1) {
		ZeroMemory(buf, _msize(buf));
		recv_size = recvfrom(sock, buf, _msize(buf), 0, NULL, 0);
		if (recv_size == SOCKET_ERROR) {
			/// ���� �� ������

			temp = WSAGetLastError();
			printf("Socket Error: %x(%d)\n", temp, temp);
		}
		
		memcpy(&totalbuf[total_size], buf, recv_size);
		total_size += recv_size;
		cout << ". ";

		// ���� ����
		if (sendto(sock, msgbuf, sizeof(msgbuf), 0, (SOCKADDR*)&sender_addr, sizeof(sender_addr)) == SOCKET_ERROR) {
			temp = WSAGetLastError();
			printf("Socket Error: %x(%d)\n", temp, temp);
		}
		
		file_size -= recv_size;
		if (file_size == 0) break;

		// �ѹ��� 60000 ������ ������ ������ ���� ���� ������ �ʿ�


		// ������ ����
		if (file_size < BUFSIZE) {
			delete[] buf;
			buf = NULL;
			buf = new char[file_size];
		}
	}



	end = clock(); // �ش� ���μ����� ���� �������� �Һ��� �ð� ��ȯ
	
	
	
	// ���� ����
	filepointer = fopen("ScreenCapture.jpeg", "wb");
	fwrite(totalbuf, 1, _msize(totalbuf), filepointer);
	fclose(filepointer);

	delete[] buf;
	buf = NULL;
	delete[] totalbuf;
	totalbuf = NULL;

	// ���� ���
	cout << "\n\n���ϼ��� �Ϸ� (Time : " << (end - start) << ")" << endl;


}