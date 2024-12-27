import tkinter as tk
from pynput.mouse import Button, Controller
from pynput import keyboard
from threading import Thread
import time

# Global Variables
running = False  # To control the autoclicker thread
mouse = Controller()

# Function to perform the autoclicking
def autoclick(button, interval):
    global running
    while running:
        mouse.click(button)
        time.sleep(interval)

# Start the autoclicker thread
def start_autoclick(button, interval_entry):
    global running
    if not running:
        try:
            interval = float(interval_entry.get())
            if interval <= 0:
                raise ValueError("Interval must be greater than 0.")
        except ValueError as e:
            status_label.config(text=f"Error: {e}", fg="red")
            return

        running = True
        status_label.config(text="Autoclicker Running", fg="green")
        thread = Thread(target=autoclick, args=(button, interval))
        thread.daemon = True  # Allows program to exit even if thread is running
        thread.start()

# Stop the autoclicker thread
def stop_autoclick():
    global running
    running = False
    status_label.config(text="Autoclicker Stopped", fg="red")

# Function to listen for the Tab key and stop autoclicking
def on_press(key):
    if key == keyboard.Key.tab:
        stop_autoclick()

# Start a thread for the keyboard listener
def start_keyboard_listener():
    listener = keyboard.Listener(on_press=on_press)
    listener.daemon = True
    listener.start()

# Create the GUI
root = tk.Tk()
root.title("Autoclicker")

# Interval Input
interval_label = tk.Label(root, text="Interval (seconds):")
interval_label.pack(pady=5)
interval_entry = tk.Entry(root)
interval_entry.pack(pady=5)
interval_entry.insert(0, "0.1")

# Buttons for Left and Right Click
start_left_button = tk.Button(root, text="Start Left Click", 
                               command=lambda: start_autoclick(Button.left, interval_entry))
start_left_button.pack(pady=5)

start_right_button = tk.Button(root, text="Start Right Click", 
                                command=lambda: start_autoclick(Button.right, interval_entry))
start_right_button.pack(pady=5)

stop_button = tk.Button(root, text="Stop", command=stop_autoclick)
stop_button.pack(pady=10)

# Status Label
status_label = tk.Label(root, text="Autoclicker Stopped", fg="red")
status_label.pack(pady=5)

# Start the keyboard listener
start_keyboard_listener()

# Run the GUI
root.mainloop()
