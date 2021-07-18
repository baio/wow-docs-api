import { EventEmitter, Output } from '@angular/core';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ActionSheetController } from '@ionic/angular';
import { TagsSelectorService } from 'src/libs/ionic/tags/services/tags-selector.service';

@Component({
    selector: 'app-doc-tags',
    templateUrl: 'doc-tags.component.html',
    styleUrls: ['doc-tags.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppDocTagsComponent {
    @Input() tags: string[];
    @Output() addTag = new EventEmitter<string>();
    @Output() removeTag = new EventEmitter<string>();
    constructor(
        private readonly tagsSelectorService: TagsSelectorService,
        private readonly actionSheetController: ActionSheetController
    ) {}

    trackByTag(_, tag: string) {
        return tag;
    }

    async onRemove(tag: string) {
        const actionSheet = await this.actionSheetController.create({
            header: 'Отвязать таг от документа',
            buttons: [
                {
                    text: 'Отвязать таг',
                    icon: 'alert-outline',
                    handler: () => {
                        this.removeTag.emit(tag);
                    },
                },
                {
                    text: 'Отмена',
                    icon: 'close-outline',
                    handler: () => {},
                },
            ],
        });

        await actionSheet.present();
    }

    async onAddTag() {
        const tag = await this.tagsSelectorService.selectTag(this.tags || []);
        if (tag) {
            this.addTag.next(tag);
        }
    }
}
