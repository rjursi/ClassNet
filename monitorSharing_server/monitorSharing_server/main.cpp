#include "server_socket_class.h"
#include <Windows.h>
#include <gdiplusbitmap.h>

void ScreenCapture() {
	HDC HandleDC = GetWindowDC(NULL);
	CImage imageObject;
	
	::SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT_SYSTEM_AWARE);

	int cx = GetSystemMetrics(SM_CXSCREEN);
	int cy = GetSystemMetrics(SM_CYSCREEN);
	int colorDepth = GetDeviceCaps(HandleDC, BITSPIXEL);

	imageObject.Create(cx, cy, colorDepth);
	StretchBlt(imageObject.GetDC(), 0, 0, cx, cy, HandleDC, 0, 0, cx, cy, SRCCOPY);

	imageObject.Save(L"ScreenCapture.jpeg", Gdiplus::ImageFormatJPEG);

	ReleaseDC(NULL, HandleDC);
	imageObject.ReleaseDC();
}

int main() {
	server_socket_class server_socket_object;

	char c_input = NULL;
	cout << "> ";
	cin >> c_input;
	if (c_input == 's') {
		system("cls");
		server_socket_object.connect_client();
		while (1) {
			ScreenCapture();
			server_socket_object.sendfile();
		}
	}

	return 0;
}