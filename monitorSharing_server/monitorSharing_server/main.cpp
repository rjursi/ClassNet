#include <iostream>
#include <atlimage.h>
#include <gdiplusbitmap.h>

#include <Windows.h>

using namespace std;

void ScreenCapture() {
	HDC HandleDC = GetWindowDC(NULL);
	CImage imageObject;

	::SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT_SYSTEM_AWARE);

	int cx = GetSystemMetrics(SM_CXSCREEN);
	int cy = GetSystemMetrics(SM_CYSCREEN);
	int colorDepth = GetDeviceCaps(HandleDC, BITSPIXEL);

	imageObject.Create(cx, cy, colorDepth);
	StretchBlt(imageObject.GetDC(), 0, 0, cx, cy, HandleDC, 0, 0, cx, cy, SRCCOPY);

	imageObject.Save(L"C://monitorSharingFTP/ScreenCapture.jpeg", Gdiplus::ImageFormatJPEG);

	ReleaseDC(NULL, HandleDC);
	imageObject.ReleaseDC();
}

int main() {
	char c_input = NULL;
	cout << "> ";
	cin >> c_input;
	if (c_input == 's') {
		system("cls");
		while (1) {
			ScreenCapture();
			system("cls");
			cout << "uploading...";
			Sleep(50);
		}
	}

	return 0;
}