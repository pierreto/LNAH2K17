import { SafeResourceUrl } from '@angular/platform-browser';

export interface MapModel {
    Id: number;
    Creator: string;
    Name: string;
    CreationDate: Date;
    Private: boolean;
    LastBackup: Date;
    Icon: SafeResourceUrl;
}
