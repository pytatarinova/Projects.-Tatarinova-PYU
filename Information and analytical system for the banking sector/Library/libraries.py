#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Библиотека универсальных функций
"""
from tkinter import filedialog as fld
import pandas as pd
import numpy as np


def read_from_text_file(file_name):
    """
    Функция читает базу данных из файла формата .csv или .xlsx
    Входные данные: имя файла (строка)
    Выходные данные: датафрейм с базой данных (pd.DataFrame())
    Автор: Якушев Тимофей
    """
    if '.csv' in file_name:  # если файл .csv
        data_local = pd.read_csv(file_name)  # чтение из .csv
    else:  # если файл .xlsx или .xls
        data_local = pd.read_excel(file_name)  # чтение из .xlsx или .xls
    return data_local  # создание и возврат датафрейма


def read_from_bin_file(file_name):
    """
    Функция читает базу данных из двоичного файла
    Входные данные: имя файла
    Выходные данные: базa данных (массив, кортеж, словарь и т. д.)
    Автор: Якушев Тимофей
    """
    data_local = np.load(file_name)  # загрузка базы данных из двоичного файла
    return data_local  # возвращение базы данных


def save_to_excel(data_local):
    """
    Функция сохраняет базу данных в файл .xlsx
    Входные данные: датафрейм с базой данных (pd.DataFrame())
    Выходные данные: нет
    Автор: Татаринова Полина
    """
    ftypes = [('Excel файлы', '*.xlsx')]  # в диалоговом окне могут отображаться только файлы .xlsx
    dlg = fld.SaveAs(filetypes=ftypes)  # вызов диалогового окна сохранения
    path = dlg.show() + '.xlsx'  # путь, выбранный пользователем
    data_local.to_excel(path, index=False)  # сохранение базы данных в формате .xlsx


def save_to_excel_index(data_local):
    """
    Функция сохраняет базу данных в файл .xlsx, оставляя индекс
    Входные данные: датафрейм с базой данных (pd.DataFrame())
    Выходные данные: нет
    Автор: Татаринова Полина
    """
    ftypes = [('Excel файлы', '*.xlsx')]
    dlg = fld.SaveAs(filetypes=ftypes)
    path = dlg.show() + '.xlsx'
    data_local.to_excel(path)  # сохранение базы данных в формате .xlsx без индекса


def save_to_csv(file_name, data_local):
    """
    Функция сохраняет базу данных в файл .csv
    Входные данные: имя файла (строка), датафрейм с базой данных (pd.DataFrame())
    Выходные данные: нет
    Автор: Якушев Тимофей
    """
    np.savetxt(file_name, data_local, fmt='%s', delimiter=';')  # сохранение базы данных в формате
    #.csv


def save_to_bin_file(data_local):
    """
    Функция сохраняет базу данных в бинарный файл
    Входные данные: датафрейм с базой данных (pd.DataFrame()), имя файла (строка)
    Выходные данные: нет
    Автор: Татаринова Полина
    """
    dlg = fld.SaveAs()#вызов диалогового окна сохранения
    path = dlg.show() #путь, выбранный пользователем
    np.save(path, data_local)  # сохранение базы данных в бинарном файле


def save_graphics(figure):
    """
    Функция сохраняет построенный график в файл .png
    Входные данные: имя файла (строка)
    Выходные данные: нет
    Автор: Пыжов Илья
    """
    ftypes = [('.png файлы', '*.png')]  # в диалоговом окне могут отображаться только файлы .png
    dlg = fld.SaveAs(filetypes=ftypes)  # вызов диалогового окна сохранения
    path = dlg.show() + '.png'  # путь, выбранный пользователем
    figure.savefig(path)  # сохранение графика в формате .png


def is_numeric(s_local):
    """
    Функция для проверки было ли введено число
    Входные данные: строка, которую нужно проверить(str)
    Выходные данные: True или False
    Автор: Пыжов Илья
    """
    try:
        float(s_local)  # пробует конвертировать в float
        return True  # возвращает True
    except ValueError:  # если возникает ошибка
        return False  # возвращает False


def plug(i):
    """
    Функция-"заглушка", чтобы при нажатии на строки treeview не срабатывали
    другие функции
    Входные данные: нет
    Выходные данные: нет
    Автор: Татаринова Полина
    """
    return i
