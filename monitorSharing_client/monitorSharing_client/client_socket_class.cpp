#include "client_socket_class.h"
#include <assert.h>
#include <time.h>
#include <Windows.h>

client_socket_class::client_socket_class() {
	WSAStartup(MAKEWORD(2, 2), &wsaData);

	if ((sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP)) == INVALID_SOCKET) {
		temp = WSAGetLastError(); 
		printf("Socket Error: %x(%d)\n", temp, temp); 
		assert(sock != INVALID_SOCKET);
	}

	setsockopt(sock, SOL_SOCKET, SO_BROADCAST, (char*)&broadcast_enable, sizeof(broadcast_enable));

	memset(&sock_addr, 0, sizeof(sock_addr));
	sock_addr.sin_family = AF_INET;
	sock_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	sock_addr.sin_port = htons(PORT);

	bind(sock, (sockaddr*)&sock_addr, sizeof(sock_addr));

	cout << "Wait for a response.";
}

client_socket_class::~client_socket_class() {
	closesocket(sock);
	WSACleanup();
}

void client_socket_class::recvfile() {
	clock_t start, end;

	if (recvfrom(sock, (char*)&file_size, sizeof(file_size), 0, NULL, 0) == SOCKET_ERROR) {
		temp = WSAGetLastError();
		printf("Socket Error: %x(%d)\n", temp, temp);
	} 
	system("cls");
	cout << "���� ũ�� : " << file_size << " Byte" << endl;

	// ���� ��ü ũ���� ���� ����
	total_size = 0;
	totalbuf = new char[file_size];
	ZeroMemory(totalbuf, _msize(totalbuf));

	cout << "\nDownloading ";
	start = clock();
	
	// ���� ����
	buf = new char[BUFSIZE];
	while (1) {
		ZeroMemory(buf, _msize(buf));
		recv_size = recvfrom(sock, buf, _msize(buf), 0, (SOCKADDR*)&sender_addr, &sender_addr_size);
		if (recv_size == SOCKET_ERROR) {
			temp = WSAGetLastError();
			printf("Socket Error: %x(%d)\n", temp, temp);
		}
		
		// ���� ����
		if (sendto(sock, msgbuf, sizeof(msgbuf), 0, (SOCKADDR*)&sender_addr, sizeof(sender_addr)) == SOCKET_ERROR) {
			temp = WSAGetLastError();
			printf("Socket Error: %x(%d)\n", temp, temp);
		}

		memcpy(&totalbuf[total_size], buf, recv_size);
		total_size += recv_size;
		cout << ". ";
		
		file_size -= recv_size;
		if (file_size == 0) break;

		// ������ ����
		if (file_size < BUFSIZE) {
			delete[] buf;
			buf = new char[file_size];
		}
	}
	end = clock();
	// ���� ����
	filepointer = fopen("ScreenCapture.jpeg", "wb");
	fwrite(totalbuf, 1, _msize(totalbuf), filepointer);
	fclose(filepointer);

	delete[] buf;
	delete[] totalbuf;

	// ���� ���
	cout << "\n\n���ϼ��� �Ϸ� (Time : " << (double)(end - start) << ")" << endl;
}