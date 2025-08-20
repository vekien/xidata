import pyautogui
import pygetwindow as gw
import time
import pyperclip

from settings import (
    send_key_delay,
    send_key_loop_delay,
    send_text_delay,
    send_window_wait_delay,
    enable_send_key_debug
)

def send_key(key, delay=None):
    if enable_send_key_debug:
        print(f"Send key: {key}")

    pyautogui.press(key)
    time.sleep(delay if delay is not None else send_key_delay)


def send_key_loop(key, count, delay=send_key_loop_delay):
    for _ in range(count):
        send_key(key, delay)


def send_text(text, delay=None, interval=send_text_delay):
    if enable_send_key_debug:
        print(f"Send text: {text}")

    pyautogui.write(text, interval=interval)
    time.sleep(delay if delay is not None else send_key_delay)


def send_alt_key(key, delay=None):
    if enable_send_key_debug:
        print(f"HOLD ALT")

    with pyautogui.hold('alt'):
        send_key(key, delay)


def send_ctrl_key(key, delay=None):
    if enable_send_key_debug:
        print(f"HOLD CTRL")
        
    with pyautogui.hold('ctrl'):
        send_key(key, delay)


def copy_text(text):
    pyperclip.copy(text)


def paste_text(text):
    copy_text(text)
    send_ctrl_key('v')


def get_active_window_title():
    win = gw.getActiveWindow()
    return win.title if win else None


def count_active_windows(window_title):
    windows = gw.getWindowsWithTitle(window_title)

    # Filter to exact case-sensitive matches
    exact_matches = [w for w in windows if w.title == window_title]

    return len(exact_matches)


def wait_for_active_window_count(window_title, delay=100, window_count=1, must_be_exact=False,stop_on_error=False):
    for _ in range(delay + 1):
        time.sleep(send_window_wait_delay)
        window_count_active = count_active_windows(window_title) 

        if enable_send_key_debug:
            print(f"Waiting for {window_count_active}/{window_count} windows titled '{window_title}'...")

        if must_be_exact and window_count_active == window_count:
            return True
        elif window_count_active >= window_count:
            return True

    print(f"!! Error: Could not find enough windows titled {window_title}")
    send_key_loop('esc', 5)  

    if stop_on_error:
        raise Exception("Stopped due to not finding active window count.")
    
    return False


def wait_for_active_window(window_title, delay=100, stop_on_error=False):
    for _ in range(delay + 1):
        time.sleep(send_window_wait_delay)

        if enable_send_key_debug:
            print(f"Waiting for windows titled '{window_title}'...")

        if get_active_window_title() == window_title:
            return True 

    print(f"!! Error: Could not detect the Window: {window_title} - Current active window: {get_active_window_title()}")
    send_key_loop('esc', 5)  

    if stop_on_error:
        raise Exception("Stopped due to not finding active window.")
    
    return False
