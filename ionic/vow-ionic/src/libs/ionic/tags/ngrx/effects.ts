import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { switchMap, map } from 'rxjs/operators';
import { TagsRepositoryService } from '../repository/tags.repository';
import {
    createTag,
    rehydrateTags,
    rehydrateTagsSuccess,
    removeTag,
} from './actions';

@Injectable()
export class TagsEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly tagsRepository: TagsRepositoryService
    ) {}

    createTag$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(createTag),
                switchMap(({ name, date }) =>
                    this.tagsRepository.addTag({ name, date })
                )
            ),
        { dispatch: false }
    );

    removeTag$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(removeTag),
                switchMap(({ name }) => this.tagsRepository.removeTag(name))
            ),
        { dispatch: false }
    );

    rehydrateTags$ = createEffect(() =>
        this.actions$.pipe(
            ofType(rehydrateTags),
            switchMap(() => this.tagsRepository.getTags()),
            map((tags) => rehydrateTagsSuccess({ tags }))
        )
    );
}
