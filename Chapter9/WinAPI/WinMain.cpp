#include <windows.h>

const char _szClassName[] = "ConcurrencyInUIWindowClass";

const UINT IDC_START_BUTTON = 101;
const UINT WM_ASYNC_TASK_COMPLETED = WM_USER + 0;

DWORD WINAPI SimulateAsyncOperation(LPVOID lpHwnd)
{
	// pretending that this is an async operation
	// posts the message to the UI message loop
	HWND hwnd = *((HWND *)lpHwnd);
	Sleep(10000);
	SendMessage(hwnd, WM_ASYNC_TASK_COMPLETED, NULL, NULL);
	return 0;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_CREATE:
		{
			HGDIOBJ hfDefault = GetStockObject(DEFAULT_GUI_FONT);
			HWND hWndButton = CreateWindowEx(NULL,
				"BUTTON",
				"OK",
				WS_TABSTOP | WS_VISIBLE |
				WS_CHILD | BS_DEFPUSHBUTTON,
				50,
				80,
				100,
				24,
				hwnd,
				(HMENU)IDC_START_BUTTON,
				GetModuleHandle(NULL),
				NULL);
			SendMessage(hWndButton,
				WM_SETFONT,
				(WPARAM)hfDefault,
				MAKELPARAM(FALSE, 0));
		}
		break;
	case WM_COMMAND:
		switch (LOWORD(wParam))
		{
			case IDC_START_BUTTON:
				{
					HANDLE threadHandle = CreateThread(NULL, 0, 
						SimulateAsyncOperation,
						&hwnd, 0, NULL);
					// we do not need a handle, so just close it
					CloseHandle(threadHandle);

					MessageBox(hwnd,
						"Start button pressed",
						"Information",
						MB_ICONINFORMATION);
				}
				break;
		}
		break;
	case WM_ASYNC_TASK_COMPLETED:
		MessageBox(hwnd,
			"Operation completed",
			"Information",
			MB_ICONINFORMATION);
		break;

	case WM_CLOSE:
		// sends WM_DESTROY
		DestroyWindow(hwnd);
		break;
	case WM_DESTROY:
		// window cleanup here
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hwnd, msg, wParam, lParam);
	}
	return 0;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	WNDCLASSEX wc;
	HWND hwnd;
	MSG Msg;

	// Creating the window class
	wc.cbSize = sizeof(WNDCLASSEX);					// size of the instance
	wc.style = 0;									// class styles, not important here
	wc.lpfnWndProc = WndProc;						// window procedure  
	wc.cbClsExtra = 0;								// extra data for this class, not relevant here
	wc.cbWndExtra = 0;								// extra data for this window, not relevant here
	wc.hInstance = hInstance;						// application instance handle
	wc.hIcon = LoadIcon(NULL, IDI_APPLICATION);		// standard large icon
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);		// standard arrow cursor
	wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);	// backround brush 
	wc.lpszMenuName = NULL;							// name of menu resource	
	wc.lpszClassName = _szClassName;				// window class name 
	wc.hIconSm = LoadIcon(NULL, IDI_APPLICATION);	// standard small icon

	if (!RegisterClassEx(&wc))
	{
		MessageBox(NULL, "Window class registration failed!", "Error!", MB_ICONEXCLAMATION | MB_OK);
		return 0;
	}

	hwnd = CreateWindowEx(
		WS_EX_CLIENTEDGE,
		_szClassName,
		"UI Concurrency",
		WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, CW_USEDEFAULT, 480, 240,
		NULL, NULL, hInstance, NULL);

	if (hwnd == NULL)
	{
		MessageBox(NULL, "Window creation failed!", "Error!", MB_ICONEXCLAMATION | MB_OK);
		return 0;
	}

	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);

	while (GetMessage(&Msg, NULL, 0, 0) > 0)
	{
		TranslateMessage(&Msg);
		DispatchMessage(&Msg);
	}
	return Msg.wParam;
}

