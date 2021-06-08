import { Injectable } from '@angular/core';
import { reject } from 'lodash';

@Injectable()
export class ImageService {
    getImageSize(url: string): Promise<{ width: number; height: number }> {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.src = url;
            img.onload = () =>
                resolve({ width: img.width, height: img.height });
            img.onerror = (err) => reject(err);
        });
    }

    compressImage(url: string, width: number, height: number): Promise<string> {
        return new Promise((res, rej) => {
            const img = new Image();
            img.src = url;
            img.onload = () => {
                const elem = document.createElement('canvas');
                elem.width = width;
                elem.height = height;
                const ctx = elem.getContext('2d');
                ctx.drawImage(img, 0, 0, width, height);
                const data = ctx.canvas.toDataURL();
                res(data);
            };
            img.onerror = (error) => rej(error);
        });
    }

    imageToBase64(file: File): Promise<string> {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result as string);
            reader.onerror = (error) => reject(error);
        });
    }

    async getImageMaxSize(url: string, max: number) {
        const size = await this.getImageSize(url);
        const [biggerSize, side] =
            size.width > size.height
                ? [size.width, 'width']
                : [size.height, 'height'];
        if (biggerSize > max) {
            if (side === 'width') {
                return { width: max, height: size.height * (max / size.width) };
            } else {
                return { width: size.width * (max / size.height), height: max };
            }
        } else {
            return size;
        }
    }

    async base64ToFile(base64: string, fileName: string, type: string) {
        const res = await fetch(base64);
        const blob = await res.blob();
        return new File([blob], fileName, { type });
    }

    async resizeImageMax(file: File, max: number) {
        const base64 = await this.imageToBase64(file);
        const maxSize = await this.getImageMaxSize(base64, max);
        const base64compressed = await this.compressImage(
            base64,
            maxSize.width,
            maxSize.height
        );
        const result = await this.base64ToFile(
            base64compressed,
            file.name,
            file.type
        );
        return {
            file: result,
            base64: base64compressed,
        };
    }
}
