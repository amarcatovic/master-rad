export default interface Post {
  id: number;
  acceptedAnswerId: number;
  answerCount: number;
  body: string;
  commentCount: number;
  creationDate: Date;
  title: string;
  viewCount: number;
  favoriteCount: number;
}
