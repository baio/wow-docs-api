export interface Tag {
    name: string;
    date: number;
}

export interface TagsState {
    tags: {
        [key: string]: Tag;
    };
}
