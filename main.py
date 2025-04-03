import os
import cv2
import subprocess


def edge_detection(img):
    """
    Функция использует алгоритм Canny для обнаружения контуров изображения, пороговое значение высчитывается с помощью
    алгоритма Оцу. Далее считается число контуров.
    Вход: img (изображение, прочитанное с помощью openCV и переведенное в оттенки серого)
    Выход: len(contours) (число контуров)
    """
    otsu_threshold, _ = cv2.threshold(img, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)
    edges = cv2.Canny(img, threshold1=otsu_threshold // 2, threshold2=otsu_threshold)
    contours, _ = cv2.findContours(edges, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
    return len(contours)


print('Введите название папки с исходными изображениями:')
original_folder = input()  # папка с исходными изображениями
while not os.path.exists(original_folder):
    print('Такой папки не существует. Введите корректное название:')
    original_folder = input()
compressed_folder = "compressed_images"  # папка со сжатыми изображениями
if not os.path.exists(compressed_folder):
    os.mkdir(compressed_folder)
diff_folder = "diff_images"  # папка с разницей изображений
if not os.path.exists(diff_folder):
    os.mkdir(diff_folder)
result_file = "results.txt"  # файл для записи результатов анализа

with open(result_file, "w") as f:  # открываем файл для записи результатов
    f.write("Имя файла | Номер зоны | Уровень помех\n")
    f.write("-" * 60 + "\n")
    for filename in os.listdir(original_folder):
        # определяем пути для оригинального, сжатого и разницы изображений, чтобы работать с ImageMagick
        original_path = os.path.join(original_folder, filename)

        compressed_filename = filename[0:filename.find('.')] + '.jpg'
        compressed_path = os.path.join(compressed_folder, compressed_filename)

        diff_filename = filename[0:filename.find('.')] + '.png'
        diff_path = os.path.join(diff_folder, compressed_filename)
        # сжимаем изображение до 40% качества в jpg
        compress = subprocess.run(['magick', 'convert', original_path, '-quality', '40%', compressed_path])
        # получаем разность изображений в png
        diff = subprocess.run(
            ['magick', 'convert', original_path, compressed_path, '-compose', 'difference', '-composite', diff_path])
        # читаем изображение с помощью openCV, преобразовываем в оттенки серого для работы с алгоритмом Canny
        diff_image = cv2.imread(diff_path)
        diff_image = cv2.cvtColor(diff_image, cv2.COLOR_BGR2GRAY)
        all_contours = edge_detection(diff_image)  # получаем общее количество контуров
        # выделяем 16 равных зон в изображении
        height, width = diff_image.shape
        zone_width = width // 4
        zone_height = height // 4
        results = []  # список в который записываются значения искажений для каждой зоны
        # подсчёт контуров в каждой зоне, запись результата в процентах
        for i in range(4):
            for j in range(4):
                x1 = i * zone_width
                y1 = j * zone_height
                x2 = x1 + zone_width
                y2 = y1 + zone_height
                zone = diff_image[y1:y2, x1:x2]
                results.append(edge_detection(zone) / all_contours * 100)
        for zone, quality in enumerate(results):  # заполнение итогового файла
            f.write(f"{filename} | Зона {zone + 1} | {quality:.2f}%\n")
        f.write(
            f"{filename} | Зона с максимальными помехами: {results.index(max(results)) + 1} | {max(results):.2f}%\n")

print("Результаты оценки качества сжатия сохранены в", result_file)
