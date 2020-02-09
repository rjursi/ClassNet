#include "server_socket_class.h"
#include <time.h>

server_socket_class::server_socket_class() {

	packet_timeToLive = TTL;


	WSAStartup(MAKEWORD(2, 2), &wsaData);

	/// ������ �����ϴ� Word ũ���, 0000001000000010 =  514
	/// wsaData : Window ���� ������ ���λ����� �����ϴ� WSADATA ������ ���� ������



	sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP);
	// PF_INET - IPv4 �ǹ�, 
	// SOCK_DGRAM - ������ �׷� ���� ����
	// IPPROTO_UDP - UDP ���� ����

	memset((char*)&groupSock, 0, sizeof(groupSock));

	groupSock.sin_family = AF_INET;
	groupSock.sin_addr.s_addr = inet_addr("239.1.1.2");
	groupSock.sin_port = htons(PORT);

	
	localInterface.s_addr = inet_addr("192.168.0.187");
	//localInterface.s_addr = inet_addr("172.18.68.161");
	//localInterface.s_addr = htonl(INADDR_ANY);
	if (setsockopt(sock, IPPROTO_IP, IP_MULTICAST_IF, (char*)&localInterface, sizeof(localInterface)) < 0) {
		perror("Setting local interface Error");
		exit(1);	
	}
	else {
		printf("Setting the local interface... OK\n");
	} 



}

server_socket_class::~server_socket_class() {
	closesocket(sock);
	WSACleanup();
}

void server_socket_class::connect_client() {
	//sendto(sock, (char*)&var_request, sizeof(var_request), 0, (const SOCKADDR*)&sock_addr, sizeof(sock_addr));


	sendto(sock, (char*)&var_request, sizeof(var_request), 0, (const SOCKADDR*)&groupSock, sizeof(groupSock));
	// ���ۿ� �ִ� ������ ��ŭ ����, �� �������� ���� (Broadcast)


	ZeroMemory(var_response, sizeof(var_response));
	// ���� ���۸� ���


	recvfrom(sock, var_response, sizeof(var_response), 0, (SOCKADDR*)&recver_addr, &recver_addr_size);
	// �������κ��� ������ �����͸� �ش� var_response ���ۿ� ����, �ش� ���� Ŭ���̾�Ʈ ������ recver_addr �� ����

}	

void server_socket_class::sendfile() {
	clock_t start, end;

	// ���� ����
	filepointer = fopen("ScreenCapture.jpeg", "rb");
	fseek(filepointer, 0, SEEK_END); // ���� ������ ��ġ�� �̵�


	file_size = ftell(filepointer);
	// ���� �������� ���� ��ġ�� ��ȯ


	rewind(filepointer);
	// ���� �����͸� ���� ������ �̵�


	sendto(sock, (char*)&file_size, sizeof(file_size), 0, (const SOCKADDR*)&recver_addr, sizeof(recver_addr));
	/// ���� ũ�⿡ ���� ������ ����

	// �ش� ������ �����͸� recver_addr ���� ������ ����

	cout << "���� ũ�� : " << file_size << " Byte" << endl;

	cout << "\nUploading ";

	start = clock();
	// ���� ����
	buf = new char[BUFSIZE];


	while (1) {
		ZeroMemory(buf, _msize(buf));

		// _msize : ���� �Ҵ��� ������ ���Ͽ� ũ�⸦ �˾ƿ��� �Լ�

		send_size = fread(buf, 1, _msize(buf), filepointer);	
		/// filepointer ���� �� _msize(buf) ���� 1����Ʈ�� �о�´�.
		/// buf �� ���� ���� ���ۿ��� ��
		// return file size

		sendto(sock, buf, _msize(buf), 0, (SOCKADDR*)&recver_addr, sizeof(recver_addr));

		// ������ ������ ��� ���۸� ����
		// �� ������ ����



		// ���� ����
		recvfrom(sock, msgbuf, sizeof(msgbuf), 0, NULL, 0);
		cout <<	 msgbuf << " ";

		file_size -= send_size;
		if (file_size == 0) break;

		// ������ ����
		if (file_size < BUFSIZE) {
			delete[] buf;
			buf = NULL;
			buf = new char[file_size];
		}


		/// ���� ���� ũ�⿡ �°� ���۸� �ٽ� �����ϰ� ����
		/// ���� �õ�

	}

	end = clock();
	fclose(filepointer);

	delete[] buf;
	buf = NULL;

	// ���� ���
	cout << "\n\n�������� ���� (Time : " << (double)(end - start) << ")\n" << endl;


}